# PostgreSQL Provisioner

## Description
Provision and manage PostgreSQL databases, users, roles, and test environments for projects with least privilege and clear operator feedback.

## When to use
Use this skill when you need to:
- create a project database
- create a test database
- create or rotate users/roles
- inspect provisioning status
- drop a project database after confirmation
- bootstrap a database for a new project

## Rules
- Never expose admin credentials to application runtime.
- Use least-privilege roles.
- Make operations idempotent where possible.
- Require explicit confirmation for destructive actions.
- Keep secrets out of normal output.
- Prefer a separate provisioning role from runtime app roles.

## Commands
- `create`
- `rotate`
- `drop`
- `status`
- `list`

## Inputs
- project name
- environment: `prod` or `test`
- optional database override
- optional user override
- password or secret reference
- bootstrap/migration flag

## Expected behavior
- Create databases and users with predictable names.
- Apply scoped grants only to the intended database.
- Support both production and test project environments.
- Report clear, concise status.
- Emit secret-hand-off instructions without printing secrets.

## File layout
```text
postgres-provisioner/
  SKILL.md
  scripts/
  references/
  tests/
```

## Workflow
1. Create the database/user pair.
2. Verify status.
3. Rotate credentials when needed.
4. Drop only after confirmation.

## Safety notes
- Use a dedicated admin/provisioner connection.
- Never hand admin credentials to app containers.
- Keep runtime credentials scoped to one database.
- Separate test DB handling from production DB handling.

## Promotion checklist
- `SKILL.md` complete
- scripts validated locally
- references current
- tests/examples included
- ready to copy from `~/code/dev-forge/skills` into the installed skill location

## Notes
This skill is reusable for any project database lifecycle, not just the FFmpeg app.
