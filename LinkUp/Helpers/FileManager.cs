using System.Text.RegularExpressions;

namespace LinkUp.Helpers
{
    public static class FileManager
    {
        private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".webp"];
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        /// <summary>
        /// Sube una imagen a wwwroot/Images/{folderName}/{id} y devuelve la ruta web ("/Images/...").
        /// Si isEditMode=true y file=null, devuelve imagePath tal como llegó (mantener imagen previa).
        /// </summary>
        public static async Task<string?> UploadAsync(
            IFormFile? file,
            string id,
            string folderName,
            bool isEditMode = false,
            string? imagePath = null,
            CancellationToken ct = default)
        {
            // Editar y no se envió nueva imagen → mantiene la anterior
            if (isEditMode && file == null)
                return imagePath;

            if (file == null || file.Length == 0)
                return string.Empty;

            // Validar tamaño
            if (file.Length > MaxFileSizeBytes)
                throw new InvalidOperationException("La imagen excede el tamaño máximo permitido.");

            // Validar extensión y MIME
            var ext = Path.GetExtension(file.FileName)?.ToLowerInvariant() ?? string.Empty;
            if (!AllowedExtensions.Contains(ext))
                throw new InvalidOperationException("Tipo de imagen no permitido.");

            if (!IsImageContentType(file.ContentType))
                throw new InvalidOperationException("Content-Type no válido para imagen.");

            // Directorio destino
            var basePath = $"Images/{SanitizePathPart(folderName)}/{SanitizePathPart(id)}";
            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", basePath);

            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            // Nombre único
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullFilePath = Path.Combine(physicalPath, fileName);

            // Guardar async
            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            // Eliminar anterior si corresponde
            if (isEditMode && !string.IsNullOrWhiteSpace(imagePath))
            {
                // imagePath esperado: "/Images/{folderName}/{id}/{oldFileName}"
                var oldPhysical = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
                if (File.Exists(oldPhysical))
                {
                    try { File.Delete(oldPhysical); } catch { /* ignore */ }
                }
            }

            // Ruta web para <img src="...">
            return $"/{basePath}/{fileName}";
        }

        /// <summary>
        /// Elimina la carpeta de imágenes de un recurso: wwwroot/Images/{folderName}/{id}
        /// </summary>
        public static bool DeleteFolder(string id, string folderName)
        {
            var basePath = $"Images/{SanitizePathPart(folderName)}/{SanitizePathPart(id)}";
            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", basePath);

            if (!Directory.Exists(physicalPath))
                return false;

            try
            {
                Directory.Delete(physicalPath, recursive: true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsImageContentType(string contentType)
        {
            // Aceptar los tipos de imagen más comunes
            return contentType is "image/jpeg" or "image/png" or "image/gif" or "image/webp";
        }

        private static string SanitizePathPart(string input)
        {
            // Evitar path traversal y caracteres inválidos en el folder/id
            var sanitized = Regex.Replace(input ?? string.Empty, @"[^a-zA-Z0-9_\-]", "_");
            return string.IsNullOrWhiteSpace(sanitized) ? "unknown" : sanitized;
        }
    }
}
