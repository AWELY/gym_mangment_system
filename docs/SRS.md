# Software Requirements Specification (SRS)

**Project:** Glory Gym Management System (`gym_mangment_system`)  
**Platform:** Microsoft .NET Framework desktop application (Windows Forms, C#)  
**Document version:** 1.0  
**Scope:** Requirements as implemented and implied by the current codebase.

---

## 1. Introduction

### 1.1 Purpose

This SRS describes the **Glory Gym Management System**: a single-user–session, Arabic-first Windows desktop application for managing gym members, subscription catalog, trainers, supplement store (POS), diet/feeding plans, basic financial summaries, and user accounts. Data is persisted locally as JSON.

### 1.2 Intended audience

- Developers maintaining or extending the system  
- Stakeholders validating scope against delivered behavior  

### 1.3 Definitions

| Term | Meaning |
|------|---------|
| **Admin** | User role with full navigation and configuration access |
| **Receptionist** | User role with restricted navigation (members, store, diet only) |
| **Snapshot** | In-memory `GymDataSnapshot` persisted to `gym_data.json` |
| **Embedded page** | Child form shown inside the dashboard content host panel |

---

## 2. Overall description

### 2.1 Product perspective

The system is a **standalone WinForms client**. It does not require a central database server; persistence uses the local file system (`%LocalApplicationData%\GloryGym\gym_data.json`). Optional integration with **Qonvo** WhatsApp API is configured via `App.config` for sending diet plan PDF links.

### 2.2 User classes and characteristics

| Actor | Description |
|-------|-------------|
| **Administrator** | Authenticated with role Admin; access to all modules, notifications, financial reports, user management, trainers, subscriptions |
| **Receptionist** | Authenticated with role Receptionist; access to Members, Store (POS), Diet plans; dashboard home hidden; starts on Members |

### 2.3 Operating environment

- **OS:** Windows (WinForms)  
- **Framework:** .NET Framework 4.8 (project targets 4.8; `App.config` references 4.7.2 runtime—align in deployment)  
- **Display:** RTL layout used on main forms for Arabic UI  
- **Optional assets:** Images under `Resources\` (e.g. background, logos) copied with the application  

### 2.4 Design and implementation constraints

- JSON serialization via `JavaScriptSerializer` with a bounded max JSON length  
- Passwords stored in plain text in local JSON (security constraint for production use)  
- Single concurrent session per process (one login → one dashboard instance)  

---

## 3. System features and functional requirements

### 3.1 Application startup and authentication

| ID | Requirement |
|----|-------------|
| **FR-AUTH-1** | On launch, the application shall initialize the data store from disk or seed defaults, load persisted UI theme preference, and display the login form (`Form1`). |
| **FR-AUTH-2** | The user shall authenticate with username and password against accounts in `GymDataStore.Data.Users` via `UserDirectory.TryAuthenticate`. |
| **FR-AUTH-3** | On successful login, the system shall set `AppSession` (role, display name) and open `DashboardForm` modally from the login form. |
| **FR-AUTH-4** | On failed login, the system shall display an Arabic error message and remain on the login form. |
| **FR-AUTH-5** | The user shall be able to exit the application from the login screen. |
| **FR-AUTH-6** | Default seeded accounts shall include at least `admin`/`admin` (Admin) and `reception`/`1234` (Receptionist) when no data file exists. |
| **FR-AUTH-7** | The login UI shall reflect the current light/dark theme and respond to global theme changes if they occur while the login form is shown. |

### 3.2 Session and sign-out

| ID | Requirement |
|----|-------------|
| **FR-SES-1** | From the dashboard, the user shall initiate sign-out; on confirmation, the dashboard shall close with a flag indicating sign-out so the login form can be shown again. |
| **FR-SES-2** | On dashboard close, the system shall persist gym data to disk (`GymDataStore.Save`). |

### 3.3 Dashboard shell

| ID | Requirement |
|----|-------------|
| **FR-DASH-1** | The dashboard shall provide a right-docked sidebar with navigation to: Home, Members, Subscriptions, Store, Diet, Financial reports, Trainers, Users (subject to role visibility). |
| **FR-DASH-2** | Receptionist role shall hide: Home, Subscriptions, Reports, Trainers, Users, and the notifications bell; status text shall indicate WhatsApp via browser (`wa.me`). |
| **FR-DASH-3** | Receptionist shall land on the Members embedded page on first load. |
| **FR-DASH-4** | The dashboard home shall show statistic cards (member counts, expiring subscriptions window, estimated monthly revenue, store sales today, trainer count, etc.) driven from `GymDataStore`. |
| **FR-DASH-5** | The dashboard shall provide a quick-actions panel with shortcuts and read-only lists (recent members, low stock, recent sales) according to role. |
| **FR-DASH-6** | The dashboard shall show a scrolling commercial banner on the top bar and a live clock on the status bar. |
| **FR-DASH-7** | The dashboard shall provide a notifications dropdown listing low stock, expiring memberships (heuristic), and recent store sales (Admin). |
| **FR-DASH-8** | Selecting a navigation item shall embed the corresponding form in the content area without a separate window frame. |
| **FR-DASH-9** | The user shall toggle **light** vs **dark** UI theme from the top bar; the choice shall persist in user-scoped application settings and apply to embedded forms and login when re-shown. |

### 3.4 Members management

| ID | Requirement |
|----|-------------|
| **FR-MEM-1** | Display members in a searchable, read-only grid (Arabic column headers). |
| **FR-MEM-2** | Support add, edit, delete member; edit/delete require row selection. |
| **FR-MEM-3** | Member fields shall include: full name, phone, gender, subscription plan (from catalog), price and duration (auto-filled from plan when possible), join date. |
| **FR-MEM-4** | Search shall filter by name and phone (client-side). |
| **FR-MEM-5** | “WhatsApp” action shall open the default browser to `wa.me` with a prefilled message for the selected member’s phone (`WhatsAppWeb`). |
| **FR-MEM-6** | Changes shall persist via `GymDataStore.Save`. |

### 3.5 Subscription plans (catalog)

| ID | Requirement |
|----|-------------|
| **FR-SUB-1** | Maintain a catalog of subscription plans (name, duration value/unit, price) stored in `GymDataStore.Data.SubscriptionPlans`. |
| **FR-SUB-2** | CRUD operations on plans shall update the grid and persist to disk. |
| **FR-SUB-3** | Plan names shall populate member subscription combo boxes elsewhere in the app. |

### 3.6 Store (POS)

| ID | Requirement |
|----|-------------|
| **FR-STR-1** | Display store products as cards (name, price, stock, expiry, category, optional photo). |
| **FR-STR-2** | Support shopping cart: add product, increment/decrement quantity, remove line. |
| **FR-STR-3** | Checkout shall decrement product stock in the data store, append a `StoreSaleRecord` (timestamp, total, summary), save, clear cart, and refresh product display. |
| **FR-STR-4** | Support adding new products (name, price, stock, expiry, category, optional image) with emoji by category convention. |
| **FR-STR-5** | Product images shall be stored as Base64 in JSON. |
| **FR-STR-6** | On store form close, the system shall persist products and dispose in-memory images as implemented. |

### 3.7 Trainers

| ID | Requirement |
|----|-------------|
| **FR-TRN-1** | Maintain trainers with: name, phone, specialty, monthly salary, join date. |
| **FR-TRN-2** | Support add, edit, delete with validation and grid binding. |
| **FR-TRN-3** | Search shall filter trainer grid by name, phone, or specialty. |

### 3.8 Users (accounts)

| ID | Requirement |
|----|-------------|
| **FR-USR-1** | Display all user accounts from `GymDataStore.Data.Users`. |
| **FR-USR-2** | Support add, edit, delete users with role Admin or Receptionist. |
| **FR-USR-3** | The built-in `admin` account shall not be deleted and shall remain Admin if edited. |
| **FR-USR-4** | Username uniqueness shall be enforced when saving. |

### 3.9 Diet / feeding plans

| ID | Requirement |
|----|-------------|
| **FR-DIE-1** | Maintain a list of feeding plans (name + PDF file path) in `GymDataStore`. |
| **FR-DIE-2** | Allow browsing and saving new plans (name + PDF path). |
| **FR-DIE-3** | Allow searching a member by phone (with autocomplete from member directory). |
| **FR-DIE-4** | Allow selecting a plan and sending it via **Qonvo** WhatsApp API when configured; append result lines to `DietSendHistory`. |
| **FR-DIE-5** | When API is not configured or fails, the system shall surface Arabic error messages as implemented. |

### 3.10 Financial reports

| ID | Requirement |
|----|-------------|
| **FR-FIN-1** | Display monthly aggregates: subscription-derived cash-in (by join month heuristic), store revenue, total trainer salaries, net after salaries. |
| **FR-FIN-2** | Render a bar chart for the current calendar year (subscription + store gross, salary bar, net per month) using GDI+ paint logic. |

### 3.11 Data persistence

| ID | Requirement |
|----|-------------|
| **FR-DAT-1** | All primary entities shall load from and save to `gym_data.json` under `%LocalApplicationData%\GloryGym\`. |
| **FR-DAT-2** | If the file is missing or invalid, the system shall seed default data and write a new file (with user-visible warning on load failure). |
| **FR-DAT-3** | The snapshot schema shall include: `Members`, `Trainers`, `Users`, `SubscriptionPlans`, `StoreProducts`, `StoreSales`, `FeedingPlans`, `DietSendHistory`, and a version field. |

### 3.12 External interfaces

| ID | Requirement |
|----|-------------|
| **FR-EXT-1** | **WhatsApp (browser):** Open `https://wa.me/` with phone and URL-encoded message for member outreach. |
| **FR-EXT-2** | **Qonvo API:** Read `QonvoBaseUrl`, `QonvoApiToken`, `QonvoSenderWhatsAppPhone`, `QonvoDefaultCountryCode` from `App.config` for authenticated diet-plan sends. |
| **FR-EXT-3** | **File system:** Open-file dialog for product images and diet PDF selection. |

### 3.13 Splash screen

| ID | Requirement |
|----|-------------|
| **FR-SPL-1** | The project includes `SplashForm` for optional startup branding/video; **current entry point** (`Program.cs`) runs `Form1` directly without displaying the splash form. Enable it only if product policy requires a splash sequence. |

---

## 4. Non-functional requirements

| ID | Category | Requirement |
|----|----------|---------------|
| **NFR-1** | Localization | Primary UI language Arabic (RTL); some labels English in title bar / window text. |
| **NFR-2** | Usability | Role-based visibility of navigation; embedded pages fill content host; theme toggle for visual accessibility. |
| **NFR-3** | Reliability | Save failures shall show a message box; load failures fall back to seeded data where implemented. |
| **NFR-4** | Performance | In-memory lists; suitable for small-to-medium club data volumes; JSON max length configured to ~8 MB. |
| **NFR-5** | Security | **Note:** Passwords in JSON are not hashed in the current implementation—production deployments should treat this as a known risk. |
| **NFR-6** | Maintainability | Central theme via `ThemeManager` / `UiColorScheme`; module forms implement `IThemeAware` where applicable. |

---

## 5. Data dictionary (logical)

| Entity | Key fields |
|--------|------------|
| **MemberRecord** | `Id`, `FullName`, `Phone`, `Gender`, `PlanName`, `PriceText`, `DurationText`, `JoinDate` |
| **TrainerRecord** | `Id`, `Name`, `Phone`, `Specialty`, `Salary`, `JoinDate` |
| **StoreProductRecord** | `Id`, `Name`, `Price`, `Category`, `Emoji`, `StockQty`, `Expiry`, `PhotoBase64` |
| **StoreSaleRecord** | `SoldAt`, `Total`, `Summary` |
| **SubscriptionPlan** | `Id`, `Name`, `Price`, `DurationValue`, `DurationUnit` |
| **UserDirectoryEntry** | `Id`, `FullName`, `Username`, `Password`, `Role` |
| **FeedingPlanRecord** | `Name`, `PdfPath` |
| **DietSendHistory** | List of string log lines |

---

## 6. Business rules (selected)

- **BR-1:** Member “expiring soon” counts on the dashboard use `MembersExpiringWithinDays`, comparing join date + plan duration (month vs year unit) to today’s date window.  
- **BR-2:** Monthly subscription “cash-in” in reports approximates revenue as members whose **join date** falls in the current month (not recurring billing).  
- **BR-3:** Store revenue by month uses `StoreSaleRecord.SoldAt` parsed as `DateTime`.  
- **BR-4:** Financial net uses sum of all trainer salaries as a monthly cost against gross (subscription + store) for the displayed month.  

---

## 7. Traceability notes

| Module | Primary code artifacts |
|--------|-------------------------|
| Login | `Form1.cs`, `UserDirectory.cs` |
| Dashboard | `DashboardForm.cs`, `DashboardForm.Designer.cs` |
| Data | `GymDataStore.cs`, `GymDataModels.cs` |
| Theme | `ThemeManager.cs`, `IThemeAware.cs`, `Properties/Settings.settings` |
| Members | `MembersForm.cs` |
| Subscriptions | `SubscriptionsForm.cs`, `SubscriptionPlanCatalog.cs` |
| Store | `StoreForm.cs` |
| Trainers | `TrainersForm.cs` |
| Users | `UsersForm.cs` |
| Diet | `DietPlanForm.cs`, `QonvoWhatsApp.cs`, `WhatsAppWeb.cs` |
| Reports | `ReportsForm.cs` |

---

## 8. Out of scope (current codebase)

- Multi-site / cloud sync  
- Encrypted or hashed credentials  
- Role-based fine-grained permissions beyond Admin vs Receptionist  
- Native mobile clients  
- Automated backups beyond local file  

---

## 9. Revision history

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2026-05-12 | Derived from repository | Initial SRS from code inspection |
