import React, { useState } from "react";
import "./App.css";

function CheckStatusButton() {
  const [message, setMessage] = useState(null);
  const checkStatus = async () => {
    try {
      const response = await fetch(
        "https://localhost:7242/api/RabbitMQ/status"
      );

      if (!response.ok) throw new Error("Network response was not ok");
      const data = await response.json();
      console.log(data);
      setMessage(response.status);
    } catch (error) {
      console.error("Error checking status", error);
    }
  };
  return (
    <div>
      <button onClick={checkStatus}>Check Status</button>
      {message !== null ? (
        <div>Status: {message}</div>
      ) : (
        <div>No status available</div>
      )}
    </div>
  );
}

function SendMessageButton() {
  const [message, setMessage] = useState(null);
  const sendMessage = async () => {
    try {
      const response = await fetch("https://localhost:7242/api/RabbitMQ/send", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ "routeKey" : "nidin.308" }),
      });
      console.log(response.json);
      setMessage(response.status);
    } catch (error) {
      console.error("Error sending message", error);
    }
  };
  return (
    <div>
      <button onClick={sendMessage}>Send Message</button>
      {message !== null ? (
        <div>Status: {message}</div>
      ) : (
        <div>No status available</div>
      )}
    </div>
  );
}

export default function App() {
  return (
    <div className="App">
      <h2>Please push this button</h2>
      <CheckStatusButton />
      <SendMessageButton />
    </div>
  );
}
