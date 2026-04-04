# PostgreSQL provisioner quickstart

## 1. Create the workspace
```bash
mkdir -p ~/code/dev-forge/skills/postgres-provisioner/{scripts,references,tests}
```

## 2. Add the skill file
Place the generated `SKILL.md` content into:
```text
~/code/dev-forge/skills/postgres-provisioner/SKILL.md
```

## 3. Add script stubs
Create:
- `create.sh`
- `rotate.sh`
- `drop.sh`
- `status.sh`
- `list.sh`

## 4. Validate locally
Run the scripts against a local Postgres instance.

## 5. Promote
Copy the finished folder into the installed skill location.

## Notes
Keep the dev-forge workspace as the source-of-truth and use the installed copy only for runtime execution.
