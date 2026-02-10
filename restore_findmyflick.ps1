Write-Host "Step 1/4: Dropping database (if it exists)..." -ForegroundColor Cyan
psql -h localhost -p 5432 -U postgres -c "DROP DATABASE IF EXISTS findmyflick WITH (FORCE);"
if ($LASTEXITCODE -ne 0) { throw "Drop failed." }

Write-Host "Step 2/4: Creating database..." -ForegroundColor Cyan
psql -h localhost -p 5432 -U postgres -c "CREATE DATABASE findmyflick;"
if ($LASTEXITCODE -ne 0) { throw "Create failed." }

Write-Host "Step 3/4: Restoring dump (this can take a few minutes)..." -ForegroundColor Cyan
pg_restore -h localhost -p 5432 -U postgres -d findmyflick -c "database/assets/findmyflick_starter.backup"
if ($LASTEXITCODE -ne 0) { throw "Restore failed." }

Write-Host "Step 4/4: Verifying row counts..." -ForegroundColor Cyan
psql -h localhost -p 5432 -U postgres -d findmyflick -c "SELECT COUNT(*) AS movies_count FROM public.movies;"
if ($LASTEXITCODE -ne 0) { throw "Verify failed." }

Write-Host "✅ Restore complete. If you see movies_count, you are ready to run backend queries." -ForegroundColor Green