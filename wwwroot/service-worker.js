const CACHE_NAME = "troupechat-static-v1";
const ASSETS_TO_CACHE = [
  "/",
  "/css/bootstrap/bootstrap.min.css",
  "/css/site.css",
  "/favicon.ico",
  "/manifest.json",
  "/web-app-manifest-192x192.png",
  "/web-app-manifest-512x512.png"
];

// Install and pre-cache static assets
self.addEventListener("install", (event) => {
  console.log("[Service Worker] Installing...");
  event.waitUntil(
    caches.open(CACHE_NAME).then((cache) => {
      return cache.addAll(ASSETS_TO_CACHE);
    })
  );
});

// Activate and clean up old caches
self.addEventListener("activate", (event) => {
  console.log("[Service Worker] Activating...");
  event.waitUntil(
    caches.keys().then((keyList) =>
      Promise.all(
        keyList.map((key) => {
          if (key !== CACHE_NAME) {
            console.log("[Service Worker] Removing old cache:", key);
            return caches.delete(key);
          }
        })
      )
    )
  );
});

// Serve cached assets when offline
self.addEventListener("fetch", (event) => {
  const requestUrl = new URL(event.request.url);

  // Don’t interfere with Blazor’s SignalR connection
  if (requestUrl.pathname.startsWith("/_blazor")) {
    return; // Let Blazor Server handle this
  }

  // Cache-first strategy for static files
  event.respondWith(
    caches.match(event.request).then((response) => {
      return response || fetch(event.request);
    })
  );
});
