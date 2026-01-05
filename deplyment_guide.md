## 📦 ATLSCAN – IIS Deployment Guide (End-to-End)

This guide walks through deploying ATLSCAN from a fresh Windows machine to a fully working IIS-hosted application, including WCF service + ASP.NET MVC UI.

### 🧩 Architecture Recap (What We Are Deploying)
```
ATLSCANService → WCF Service (IIS-hosted)

ALTSCANUI → ASP.NET MVC Web App

File System → SourceZips, Destination, Logs

Both applications will be hosted under IIS.
```
### 🖥️ 1. Prerequisites
```
Operating System

Windows 10 / 11 / Windows Server 2019+

Software

.NET Framework 4.8

IIS (Internet Information Services)

Visual Studio (for publishing)
```
### 🔧 2. Enable Required Windows Features
```
Open Windows Features
Control Panel → Programs → Turn Windows features on or off

Enable the following:
✅ Internet Information Services

Web Management Tools

✔ IIS Management Console

  World Wide Web Services

    Application Development Features

      ✔ .NET Extensibility 4.8

      ✔ ASP.NET 4.8

      ✔ ISAPI Extensions

      ✔ ISAPI Filters

  Common HTTP Features

    ✔ Default Document

    ✔ Static Content

  Security

    ✔ Request Filtering

✅ WCF Services

  .NET Framework 4.8 Advanced Services

    ✔ WCF Services

    ✔ HTTP Activation

📌 Important:
If WCF HTTP Activation is not enabled, .svc files will NOT work.

➡ Click OK and restart the machine if prompted.
```
### 🌐 3. Verify IIS Installation
```
Open Run

Type:

inetmgr


IIS Manager should open

Browse:

http://localhost


You should see the IIS welcome page.
```
### 📂 4. Prepare Folder Structure (Recommended)
```
Create a deployment root:

C:\ATLSCAN\
│
├── UI\
├── Service\
├── SourceZips\
├── Destination\
└── Logs\


These paths will be referenced by both UI and service.
```
### 🧪 5. Publish ATLSCANService (WCF)
```
Step 1: Publish from Visual Studio

Open ATLSCANService project

Right-click → Publish

Choose:

Folder


Target location:

C:\ATLSCAN\Service


Click Publish

You should see:

ZipService.svc

bin/ATLSCANService.dll

Web.config

Step 2: Create IIS Application for WCF

Open IIS Manager

Expand Sites → Default Web Site

Right-click → Add Application

Field	Value
Alias	ATLSCANService
Physical Path	C:\ATLSCAN\Service
Application Pool	DefaultAppPool

Click OK

Step 3: Configure Application Pool

Select Application Pools

Select pool used by ATLSCANService

Set:

.NET CLR Version: v4.0

Managed Pipeline Mode: Integrated

Advanced Settings:

Enable 32-Bit Applications: False

Step 4: Test WCF Service

Open browser:

http://localhost/ATLSCANService/ZipService.svc


✅ You should see WCF Service Help Page

❌ If you see download prompt → HTTP Activation not enabled
```
### 🖥️ 6. Publish ALTSCANUI (ASP.NET MVC UI)
```
Step 1: Publish UI

Right-click ALTSCANUI

Click Publish

Target:

C:\ATLSCAN\UI


Publish

Step 2: Create IIS Application for UI

IIS Manager → Default Web Site

Right-click → Add Application

Field	Value
Alias	ATLSCANUI
Physical Path	C:\ATLSCAN\UI
Application Pool	Same or separate pool
Step 3: Configure Application Pool

.NET CLR Version: v4.0

Pipeline Mode: Integrated

Identity: ApplicationPoolIdentity
```
### 🔗 7. Configure WCF Endpoint in UI
```
Open:

C:\ATLSCAN\UI\Web.config


Verify endpoint points to IIS-hosted service:

<endpoint
  address="http://localhost/ATLSCANService/ZipService.svc"
  binding="basicHttpBinding"
  contract="AtlscanRef.IZipService" />


📌 If hosting on server, replace localhost with server hostname.
```
### 🔐 8. Folder Permissions (CRITICAL)
```
Grant Modify permissions to IIS App Pool identity:

Folders:

C:\ATLSCAN\SourceZips

C:\ATLSCAN\Destination

C:\ATLSCAN\Logs

Steps:

Right-click folder → Properties

Security → Edit

Add:

IIS AppPool\<YourAppPoolName>


Grant:

Read

Write

Modify
```
### ▶️ 9. Final Validation
```
Test UI
http://localhost/ATLSCANUI

Test Flow

Upload / place ZIP in SourceZips

Manual ZIP processing

Batch processing

Search extracted files

Verify logs

❗ Common Issues & Fixes
❌ 404 on .svc

✔ Enable WCF HTTP Activation

❌ 500.19 Error

✔ Check Web.config
✔ Ensure .NET 4.8 installed

❌ Access Denied

✔ Fix folder permissions

❌ UI loads but processing fails

✔ Ensure WCF service is running
✔ Check endpoint URL

✅ Production Hardening (Optional)

Use separate App Pools for UI & Service

Enable Failed Request Tracing

Add logging rotation

Bind HTTPS

Use Windows Authentication if internal
```
### 🎯 Result
```
You now have:

IIS-hosted WCF service

IIS-hosted ASP.NET MVC UI

Fully working ZIP processing system
```
