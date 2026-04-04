# Dev-forge workspace layout for PostgreSQL provisioner

## Target workspace
`~/code/dev-forge/skills/postgres-provisioner`

## Copy-ready structure

```text
postgres-provisioner/
  SKILL.md
  scripts/
    create.sh
    rotate.sh
    drop.sh
    status.sh
    list.sh
  references/
    postgres-notes.md
    examples.md
  tests/
    smoke.md
```

## Copy steps
1. Create the workspace folder tree.
2. Place the `SKILL.md` content into `SKILL.md`.
3. Add the script stubs.
4. Add reference notes and example usage.
5. Add smoke tests.
6. Validate locally.
7. Copy the finished folder into the installed skill location.

## What to keep in the source workspace
- source `SKILL.md`
- scripts
- references
- tests
- version notes

## What to promote
- only the finished skill folder
- not draft scratch files
- not temporary test artifacts
