# DB-backed host state plan

## Goal
Move the host from in-memory state to simple file-backed persistence now, then evolve to SQLite/Postgres without changing the UI/core flow.

## Immediate implementation
- add a small storage service in the host
- persist jobs and presets as JSON under a data directory
- load existing state at startup
- keep defaults as a fallback when no data exists

## Next step after that
- replace JSON files with SQLite tables
- map jobs, runs, presets, and app settings into relational models
- keep the core library unchanged

## Why this is a good intermediate step
- low risk
- easy to inspect and back out
- no DB schema work required yet
- makes the host feel persistent before moving to a real database
