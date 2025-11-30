// -------------------------
// Global Variables
// -------------------------
let connection;
let username = "";

// -------------------------
// Backend Status Checker
// -------------------------
function checkBackend() {
  fetch("http://localhost:5276/api/questions/random?mode=Algorithm")
    .then((res) => {
      document.querySelectorAll(".connection-status").forEach((el) => {
        el.textContent = "Backend: Connected";
        el.style.color = "rgba(0, 78, 0, 1)";
      });
    })
    .catch((err) => {
      document.querySelectorAll(".connection-status").forEach((el) => {
        el.textContent = "Backend: Disconnected";
        el.style.color = "#f00";
      });
    });
}

// Ping every 3 seconds
setInterval(checkBackend, 3000);
checkBackend();

// Login Page
// -------------------------
const guestBtn = document.getElementById("playGuest");
const signInBtn = document.getElementById("signIn");

if (guestBtn) {
  guestBtn.onclick = () => {
    const guestName = document.getElementById("guestUsername").value.trim();
    if (!guestName) return alert("Enter a username to play as guest");
    username = guestName;
    localStorage.setItem("username", username);
    window.location.href = "game-mode.html";
  };
}

if (signInBtn) {
  signInBtn.onclick = () => {
    const u = document.getElementById("username").value.trim();
    const p = document.getElementById("password").value.trim();
    if (!u || !p) return alert("Enter username and password");
    // Demo only: no backend auth yet
    username = u;
    localStorage.setItem("username", username);
    window.location.href = "game-mode.html";
  };
}

// -------------------------
// Game Mode Page
// -------------------------
const usernameDisplay = document.getElementById("usernameDisplay");
if (usernameDisplay) {
  username = localStorage.getItem("username") || "Player";
  usernameDisplay.textContent = username;

  const algoBtn = document.getElementById("algoMode");
  const cyberBtn = document.getElementById("cyberMode");

  if (algoBtn)
    algoBtn.onclick = () => {
      localStorage.setItem("gamemode", "Algorithm");
      window.location.href = "game.html";
    };
  if (cyberBtn)
    cyberBtn.onclick = () => {
      localStorage.setItem("game-mode", "CyberBomb");
      window.location.href = "game.html";
    };
}

// -------------------------
// Game Page
// -------------------------
if (document.getElementById("questionBox")) {
  username = localStorage.getItem("username") || "Player";
  const mode = localStorage.getItem("gamemode") || "Algorithm";

  // Display username
  const usernameEl = document.getElementById("usernameDisplay");
  if (usernameEl) usernameEl.textContent = username;

  const backendStatus = document.getElementById("backendStatus");
  const questionText = document.getElementById("questionText");
  const answersDiv = document.getElementById("answers");
  const scoreSpan = document.getElementById("score");
  const streakSpan = document.getElementById("streak");
  const chatMessages = document.getElementById("chatMessages");

  // Connect to SignalR
  connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5276/gamehub")
    .withAutomaticReconnect()
    .build();

  connection
    .start()
    .then(() => {
      backendStatus.textContent = "Backend: Connected";
      backendStatus.style.color = "#0f0";
      // Join a test game room (gameId = 1 for demo)
      connection.invoke("JoinGame", 1, 1); // userId = 1 for demo
      // Request first question
      connection.invoke("RequestQuestion", mode);
    })
    .catch((err) => {
      backendStatus.textContent = "Backend: Disconnected";
      backendStatus.style.color = "#f00";
    });

  // -------------------------
  // SignalR Event Handlers
  // -------------------------
  connection.on("ReceiveQuestion", (q) => {
    questionText.textContent = q.QuestionText;
    answersDiv.innerHTML = "";

    // Demo multiple-choice A-D
    const options = ["A", "B", "C", "D"];
    options.forEach((opt) => {
      const btn = document.createElement("button");
      btn.textContent = opt;
      btn.onclick = () => {
        connection.invoke("AnswerQuestion", 1, 1, q.Id, opt);
      };
      answersDiv.appendChild(btn);
    });
  });

  connection.on("AnsweredCorrect", (res) => {
    scoreSpan.textContent = res.newScore;
    streakSpan.textContent = res.newStreak;
  });

  connection.on("AnsweredWrong", (res) => {
    scoreSpan.textContent = res.newScore;
    streakSpan.textContent = res.newStreak;
    if (res.BanApplied) alert(`You are banned for ${res.BanSeconds} seconds!`);
  });

  // Chat
  document.getElementById("sendChat").onclick = () => {
    const msgInput = document.getElementById("chatInput");
    const msg = msgInput.value.trim();
    if (!msg) return;
    connection.invoke("SendChat", 1, 1, msg);
    msgInput.value = "";
  };

  connection.on("ReceiveChat", (userId, message) => {
    const msgDiv = document.createElement("div");
    msgDiv.textContent = message;
    chatMessages.appendChild(msgDiv);
    chatMessages.scrollTop = chatMessages.scrollHeight;
  });
}
