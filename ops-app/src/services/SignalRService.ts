import * as signalR from "@microsoft/signalr";
import { Constants } from "../constants";
// ✅ Ask for Notification Permission
const requestNotificationPermission = () => {
    if (Notification.permission !== "granted") {
        Notification.requestPermission().then((permission) => {
            console.log("Notification permission:", permission);
        });
    }
};

// ✅ Create SignalR connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl(Constants.SIGNALR_URL) // Replace with your actual SignalR hub URL
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
    .build();

// ✅ Start connection with retry logic
const startConnection = async () => {
    try {
        await connection.start();
        console.log("SignalR Connected");
    } catch (err) {
        console.error("SignalR Connection Error:", err);
        setTimeout(startConnection, 5000); // Retry after 5 seconds
    }
};

// ✅ Show browser notification when receiving a poll update
const showNotification = (pollId: number) => {
    if (Notification.permission === "granted") {
        new Notification("Poll Updated", {
            body: `Poll ${pollId} has been updated!`,
            icon: "/vite.svg", // Optional icon
        });
    }
};

// ✅ Subscribe to SignalR Poll Updates
const subscribeToPollUpdates = (callback: (pollId: number) => void) => {
    connection.on("ReceivePollUpdate", (pollId: number) => {
        console.log("Poll Updated:", pollId);
        showNotification(pollId);
        callback(pollId);
    });
};

// Start connection and request notification permission
requestNotificationPermission();
startConnection();

export { connection, subscribeToPollUpdates };
