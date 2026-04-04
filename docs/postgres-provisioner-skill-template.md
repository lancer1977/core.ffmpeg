# PostgreSQL Provisioner Skill Template

## Name
PostgreSQL Provisioner

## Purpose
Create and manage PostgreSQL databases and roles for projects and test environments in a repeatable, least-privilege way.

## When to use
Use this skill when you need to:
- create a project database
- create a test database
- create or rotate users/roles
- drop an unused project database
- inspect provisioning status
- bootstrap a database for a new project

## What the skill should do
- accept a project name and environment
- create databases and users with predictable names
- apply least-privilege grants
- support test database creation
- avoid printing secrets by default
- emit clear operator-friendly status

## Suggested command set
- create
- rotate
- drop
- status
- list

## Inputs
- project name
- environment (prod/test)
- database name override
- user name override
- password or secret reference
- bootstrap/migration flag

## Outputs
- confirmation of created resources
- generated names for DB/user/role
- secret handoff details
- warnings when resources already exist

## Safety rules
- never expose the provisioning admin credentials to application runtime
- use least-privilege roles
- prefer idempotent operations
- require explicit confirmation for destructive actions like drop

## Suggested files
- `SKILL.md`
- `scripts/`
- `references/`
- `tests/`

## Example operator flow
1. `create` the project DB/user.
2. `status` to confirm.
3. `rotate` when needed.
4. `drop` only with explicit confirmation.

## Notes for OpenClaw
This should remain separate from the FFmpeg app itself and be reusable for other project databases too.
