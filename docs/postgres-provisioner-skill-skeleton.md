# PostgreSQL Provisioner Skill

## Name
PostgreSQL Provisioner

## Description
Provision and manage project PostgreSQL databases, users, and test environments with least privilege.

## Use when
- creating a new project database
- creating a test database
- rotating database credentials
- checking provisioning status
- tearing down a project DB

## Rules
- never expose admin credentials to app runtime
- use least-privilege roles
- make operations idempotent where possible
- require confirmation for destructive actions
- keep secrets out of normal output

## Inputs
- project name
- environment (`prod` or `test`)
- optional database/user overrides
- optional secret/password reference
- optional bootstrap/migration flag

## Commands
- `create`
- `rotate`
- `drop`
- `status`
- `list`

## Expected behavior
- create databases and users with predictable names
- grant access only to the intended database
- support test database creation separately from prod
- report clear operator-friendly output

## File layout
```text
postgres-provisioner/
  SKILL.md
  scripts/
  references/
  tests/
```

## Example flow
1. `create` the project database.
2. `status` to verify.
3. `rotate` credentials when needed.
4. `drop` only after explicit confirmation.

## Notes
This skill is separate from the FFmpeg app and should remain reusable for other projects.
