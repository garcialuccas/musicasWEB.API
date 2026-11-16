using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using musicasWEB.API.Interfaces;
using musicasWEB.API.Utils;
using System.Text.RegularExpressions;

namespace musicasWEB.API.Services
{
    public class CloudinaryService : IImagemService
    {
        private readonly CloudinarySettings _settings;
        private readonly ILogger<CloudinaryService> _logger;
        private readonly Cloudinary? _cliente;

        public CloudinaryService(IOptions<CloudinarySettings> options, ILogger<CloudinaryService> logger)
        {
            _settings = options.Value;
            _logger = logger;

            if (IsConfigured) _cliente = new Cloudinary(new Account(_settings.CloudName, _settings.ApiKey, _settings.ApiSecret));

        }

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(_settings.ApiKey) &&
            !string.IsNullOrWhiteSpace(_settings.ApiSecret) &&
            !string.IsNullOrWhiteSpace(_settings.CloudName);

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            if (_cliente is null
                || !Uri.TryCreate(imageUrl, UriKind.Absolute, out var url)
                || url.Scheme != Uri.UriSchemeHttps
                || !string.Equals(url.Host, "res.cloudinary.com", StringComparison.OrdinalIgnoreCase)) 
            {
                return false;
            }

            var pattern = $@"^/(?i:{Regex.Escape(_settings.CloudName)})/image/upload/v\d+/(plot/(?:Musicas|Albuns|Artistas)/[0-9a-fA-F]{{32}})\.[^/]+$";
            var match = Regex.Match(url.AbsolutePath, pattern);

            if (!match.Success) return false;

            var publicId = match.Groups[1].Value;

            try
            {
                var result = await _cliente.DestroyAsync(new CloudinaryDotNet.Actions.DeletionParams(publicId));
                return result.Result == "ok";
            }
            catch (Exception erro)
            {
                _logger.LogWarning(erro, "Não foi possível remover a imagem.");
                return false;
            }
        }

        public async Task<string> UploadImageAsync(IFormFile arquivo, ImageFolder pasta)
        {
            if (_cliente is null)
                throw new InvalidOperationException("Configure as credenciais do Cloudinary.");

            if (arquivo is null || arquivo.Length is <= 0 or > 10 * 1024 * 1024)
                throw new ArgumentException("A imagem deve ter entre 1 byte e 10 MB.", nameof(arquivo));

            var extensao = Path.GetExtension(arquivo.FileName).ToLowerInvariant();
            if (extensao is not (".jpg" or ".jpeg" or ".png" or ".webp" or ".gif"))
                throw new ArgumentException("Use jpg, png, webp, jpeg ou gif.", nameof(arquivo));

            var pastaCloud = pasta switch
            {
                ImageFolder.Musicas => "msc/musicas",
                ImageFolder.Albuns => "msc/albuns",
                ImageFolder.Artistas => "msc/artistas",
                _ => throw new ArgumentOutOfRangeException(nameof(pasta))
            };

            using var dados = arquivo.OpenReadStream();
            ImageUploadResult resultado;

            try
            {
                resultado = await _cliente.UploadAsync(new ImageUploadParams
                {
                    File = new FileDescription(arquivo.FileName, dados),
                    Folder = pastaCloud,
                    PublicId = Guid.NewGuid().ToString("N"),
                    Overwrite = false
                });
            }
            catch (Exception erro)
            {
                throw new HttpRequestException("Falha ao comunicar com o Cloudinary.", erro);
            }

            return resultado.Error is null && resultado.SecureUrl is not null
                ? resultado.SecureUrl.ToString()
                : throw new HttpRequestException("O Cloudinary não conseguiu salvar a imagem.");
        }
    }
}
