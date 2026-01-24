# FindMyFlick

FindMyFlick is a web app that helps US-based users find US-based streaming options across multiple platforms using filters like genre, streaming service, actor/director, and content warnings.

## Quick start: restore the database (recommended)

This project includes a starter Postgres database snapshot so you do not have to run the schema/seed scripts.

## Prereqs: install Postgres and confirm your terminal tools work

### 1. Install PostgreSQL (local install)
Download and install PostgreSQL (recommended: version 16). During installation:
- Set a password for the `postgres` user (write it down).
- Keep the default port `5432` unless you already use it for something else.

After install, you should have:
- A running Postgres service on your machine
- Command-line tools: `psql`, `pg_dump`, `pg_restore`

### 2. Confirm the Postgres service is running
Windows:
- Open Services (search "Services")
- Look for `postgresql-x64-16` (version may differ)
- Status should be **Running**

Mac (Homebrew install):
- `brew services list | grep postgresql`

### 3. Ensure Postgres command-line tools are available in your terminal
From VS Code PowerShell (repo root), run:

```powershell
psql --version
pg_dump --version
pg_restore --version
```

If any of these say "not recognized", you need to add Postgres `bin` to your PATH.

Typical PATH location on Windows (adjust version number):

- `C:\Program Files\PostgreSQL\16\bin`

After updating PATH:

1. Close VS Code completely  
2. Reopen VS Code  
3. Re-run the version commands above  

### 4. Confirm you can connect as the `postgres` user

In VS Code PowerShell:

```powershell
psql -U postgres
```

If it prompts for a password, enter the password you set during installation.

You should see a prompt like:

- `postgres=#`

Exit psql:

```sql
\q
```

### 5. Run the database restore script

From the repository root, run the restore script:

```powershell
.\restore_findmyflick.ps1
```

When it finishes, it prints a movie count and a success message.

If PowerShell blocks scripts, run this first (same terminal window), then rerun the script:

```powershell
Set-ExecutionPolicy -Scope Process Bypass
```

---

# React + Vite

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Babel](https://babeljs.io/) (or [oxc](https://oxc.rs) when used in [rolldown-vite](https://vite.dev/guide/rolldown)) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/) for Fast Refresh

## React Compiler

The React Compiler is not enabled on this template because of its impact on dev & build performances. To add it, see [this documentation](https://react.dev/learn/react-compiler/installation).

## Expanding the ESLint configuration

If you are developing a production application, we recommend using TypeScript with type-aware lint rules enabled. Check out the [TS template](https://github.com/vitejs/vite/tree/main/packages/create-vite/template-react-ts) for information on how to integrate TypeScript and [`typescript-eslint`](https://typescript-eslint.io) in your project.


---

## References

- Instructions for restoring the database were written with the assistance of ChatGPT.