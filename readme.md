
## **📌 Online Polling System**

A full-stack online polling application with a **React frontend (Vite)**, **ASP.NET Core API**, and **SQL Server**, running via **Docker Compose**.

---

## **🚀 Features**

✅ Create and vote on polls  
✅ Real-time poll updates using **SignalR**  
✅ Data persistence with **SQL Server**  
✅ Full stack runs using **Docker Compose**  

---

## **📂 Project Structure**

```
OnlinePollingSystem/
|── PollingApp/                # MAUI framework (Mobile App)
│── ops-app/                   # React Frontend (Web App)
│── OnlinePollingSystem/       # Backend (ASP.NET Core API)
│── docker-compose.yml         # Docker Compose for full stack
│── .gitignore                 # Ignore unnecessary files
│── README.md                  # Documentation
│── sqlserver-data/            # Persistent SQL Server data volume
```

---

## **🔧 Prerequisites**

Ensure you have the following installed:

- **Docker & Docker Compose** → [Download](https://www.docker.com/get-started)  
- **Git** → [Download](https://git-scm.com/downloads)  
- **.NET SDK (if running API locally)** → [Download](https://dotnet.microsoft.com/download)  
- **Node.js & npm (if running React locally)** → [Download](https://nodejs.org/)  

---

## **🛠️ Setup & Run with Docker (Recommended)**

1️⃣ Clone the repository:

```sh
git clone https://github.com/naveen-bathina/OnlinePollingSystem.git
cd OnlinePollingSystem
```

2️⃣ Run the project using Docker Compose:

```sh
docker-compose up --build
```

3️⃣ Access the services:

- **API** → `http://localhost:8090`
- **Frontend (React App)** → `http://localhost:5173`
- **SQL Server (Docker)** → Runs on port `1433`

---

## **💻 Running Locally Without Docker**

### 1️⃣ **Run Backend (ASP.NET Core)**

```sh
cd OnlinePollingSystem/Ops.Api
dotnet restore
dotnet run
```

API should now be running at **`http://localhost:5000`**

### 2️⃣ **Run Frontend (React, Vite)**

```sh
cd ops-app
npm install
npm run dev
```

React app should now be running at **`http://localhost:5173`**

---

## **🐳 Managing Docker Containers**

| Command | Description |
|---------|------------|
| `docker-compose up --build` | Build & start all containers |
| `docker-compose up -d` | Start containers in the background |
| `docker-compose down` | Stop all containers |
| `docker-compose logs -f` | View logs |
| `docker system prune -a` | Clean up unused Docker images & containers |

---

## **⚠️ Troubleshooting**

### ❌ **Port Already in Use?**

Run:

```sh
netstat -ano | findstr :PORT
taskkill /PID <PID> /F
```

Replace `PORT` with `8090`, `5173`, or `1433`.

### ❌ **Docker Volume Issues?**

Remove volumes & restart:

```sh
docker-compose down -v
docker-compose up --build
```

### ❌ **React App Not Loading?**

Try:

```sh
cd ops-app
rm -rf node_modules package-lock.json
npm install
npm run dev
```

---

## **📜 License**

This project is licensed under **MIT License**.

---

## **✉️ Contact**

For any questions, feel free to reach out  !
Naveen Bathina | <naveen.bathina24@outlook.com> | +91 962 043 0945

---

## Thank you
