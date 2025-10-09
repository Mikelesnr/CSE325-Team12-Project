// Notification functions for real-time chat

// Play notification sound
window.playNotificationSound = function () {
    try {
        // Create a simple notification beep using Web Audio API
        const audioContext = new (window.AudioContext || window.webkitAudioContext)();
        const oscillator = audioContext.createOscillator();
        const gainNode = audioContext.createGain();

        oscillator.connect(gainNode);
        gainNode.connect(audioContext.destination);

        oscillator.frequency.value = 800; // Frequency in Hz
        oscillator.type = 'sine';

        gainNode.gain.setValueAtTime(0.3, audioContext.currentTime);
        gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.5);

        oscillator.start(audioContext.currentTime);
        oscillator.stop(audioContext.currentTime + 0.5);
    } catch (error) {
        console.log('Audio notification not supported:', error);
    }
};

// Show browser notification
window.showBrowserNotification = function (senderName, content) {
    // Check if browser supports notifications
    if (!("Notification" in window)) {
        console.log("This browser does not support notifications");
        return;
    }

    // Check if permission is granted
    if (Notification.permission === "granted") {
        createNotification(senderName, content);
    }
    // Request permission if not denied
    else if (Notification.permission !== "denied") {
        Notification.requestPermission().then(function (permission) {
            if (permission === "granted") {
                createNotification(senderName, content);
            }
        });
    }
};

// Create the notification
function createNotification(senderName, content) {
    const notification = new Notification(`New message from ${senderName}`, {
        body: content.substring(0, 100), // Limit content length
        icon: '/favicon.png',
        badge: '/favicon.png',
        tag: 'chat-message',
        requireInteraction: false,
        silent: false
    });

    // Auto-close after 5 seconds
    setTimeout(() => notification.close(), 5000);

    // Focus window when notification is clicked
    notification.onclick = function () {
        window.focus();
        notification.close();
    };
}

// Update page title with unread indicator
window.updatePageTitle = function (title) {
    document.title = title;
};

// Reset page title
window.resetPageTitle = function () {
    document.title = "Chat - Troupe Chat";
};

// Request notification permission on page load
window.requestNotificationPermission = function () {
    if ("Notification" in window && Notification.permission === "default") {
        Notification.requestPermission();
    }
};

// Add visual notification badge
window.showNotificationBadge = function (count) {
    // Update favicon with badge (optional enhancement)
    // This is a placeholder for future enhancement
    console.log(`Unread messages: ${count}`);
};

