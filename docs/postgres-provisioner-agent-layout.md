# PostgreSQL provisioner agent layout

## Recommended files
- `SKILL.md` — main skill definition
- `scripts/create.sh` — create database/user
- `scripts/rotate.sh` — rotate credentials
- `scripts/drop.sh` — remove resources
- `scripts/status.sh` — inspect current state
- `scripts/list.sh` — list managed projects
- `references/` — command examples and postgres notes
- `tests/` — smoke tests or CLI validation

## Development flow
1. Draft the skill in `~/code/dev-forge/skills/postgres-provisioner`
2. Add scripts and references
3. Validate manually against a local Postgres instance
4. Copy into the installed skill location
5. Tag or version the source copy

## Suggested testing
- dry-run creation
- idempotent re-run
- password rotation
- drop confirmation
- status checks for existing and missing resources
