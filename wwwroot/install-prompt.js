let deferredPrompt;
console.log("Install prompt script loaded");

window.addEventListener("beforeinstallprompt", (e) => {
  e.preventDefault();
  deferredPrompt = e;

  const installBtn = document.getElementById("installBtn");
  if (!installBtn) return;

  installBtn.style.display = "block";
  installBtn.addEventListener("click", () => {
    installBtn.style.display = "none";
    deferredPrompt.prompt();

    deferredPrompt.userChoice.then((choice) => {
      if (choice.outcome === "accepted") {
        console.log("TroupeChat installed successfully");
      } else {
        console.log("TroupeChat installation dismissed");
      }
      deferredPrompt = null;
    });
  });
});
