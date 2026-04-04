# Dev-forge workspace skeleton for PostgreSQL provisioner

## Workspace
`~/code/dev-forge/skills/postgres-provisioner`

## Suggested tree

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

## SKILL.md outline
- name
- description
- when to use
- rules
- commands
- workflow
- safety notes
- promotion checklist

## Script behavior
- `create.sh`: create project DB/user and set grants
- `rotate.sh`: rotate the password and update secrets
- `drop.sh`: remove DB/user after confirmation
- `status.sh`: inspect existence and grants
- `list.sh`: enumerate managed projects

## References
- naming conventions
- role model
- local Postgres connection notes
- secret store handoff notes

## Tests
- idempotent create
- rotate succeeds
- drop requires confirmation
- status reflects missing/existing resources

## Promotion
When complete, copy the finished skill into the installed skill directory and keep the dev-forge copy as the source-of-truth.
