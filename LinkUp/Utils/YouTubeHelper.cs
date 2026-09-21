using System.Web;

namespace LinkUp.Utils
{
    public class YouTubeHelper
    {

       
        
            /// <summary>
            /// Intenta extraer el videoId desde múltiples formatos:
            /// - https://www.youtube.com/watch?v=VIDEOID
            /// - https://youtu.be/VIDEOID
            /// - https://www.youtube.com/embed/VIDEOID
            /// - https://www.youtube.com/shorts/VIDEOID
            /// - También soporta parámetros extra (&t=, &ab_channel=, etc.)
            /// </summary>
            public static bool TryGetVideoId(string url, out string videoId)
            {
                videoId = string.Empty;
                if (string.IsNullOrWhiteSpace(url)) return false;

                // Normalizamos
                url = url.Trim();

                // 1) youtu.be/VIDEOID
                // 2) youtube.com/embed/VIDEOID
                // 3) youtube.com/shorts/VIDEOID
                // 4) youtube.com/watch?v=VIDEOID
                try
                {
                    if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
                    {
                        var host = uri.Host.ToLowerInvariant();

                        // youtu.be/<id>
                        if (host.EndsWith("youtu.be"))
                        {
                            var seg = uri.AbsolutePath.Trim('/');
                            if (!string.IsNullOrEmpty(seg))
                            {
                                videoId = seg;
                                return true;
                            }
                        }

                        // youtube dominios
                        if (host.Contains("youtube.com"))
                        {
                            var path = uri.AbsolutePath.Trim('/').ToLowerInvariant();

                            // /embed/<id>  ó  /shorts/<id>
                            if (path.StartsWith("embed/") || path.StartsWith("shorts/"))
                            {
                                var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                                if (parts.Length >= 2)
                                {
                                    videoId = parts[1];
                                    return true;
                                }
                            }

                            // /watch?v=<id>
                            if (path == "watch")
                            {
                                var query = HttpUtility.ParseQueryString(uri.Query);
                                var v = query.Get("v");
                                if (!string.IsNullOrWhiteSpace(v))
                                {
                                    videoId = v;
                                    return true;
                                }
                            }
                        }
                    }

                    // Si llega solo el ID (caso extremo)
                    if (url.Length >= 10 && url.Length <= 20 && url.All(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == '_'))
                    {
                        videoId = url;
                        return true;
                    }
                }
                catch
                {
                    // ignoramos y devolvemos false
                }

                return false;
            }

            /// <summary>
            /// Retorna la URL de embed si es válido; de lo contrario null.
            /// </summary>
            public static string? ToEmbedUrl(string? url)
            {
                if (string.IsNullOrWhiteSpace(url)) return null;
                return TryGetVideoId(url, out var id)
                    ? $"https://www.youtube.com/embed/{id}"
                    : null;
            }
        }
    }


