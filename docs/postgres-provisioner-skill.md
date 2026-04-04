# PostgreSQL Provisioner Skill (draft)

Use this as the working definition for the dev-forge skill.

## Short description
Provision project PostgreSQL databases, users, and test environments with least privilege.

## Primary commands
- create
- rotate
- drop
- status
- list

## Behavior
- project-scoped database/user naming
- safe credential rotation
- explicit confirmation for destructive actions
- no secret leakage in normal output

## Files
- `SKILL.md`
- `scripts/`
- `references/`
- `tests/`

## Workflow
- build in `~/code/dev-forge/skills`
- validate locally
- copy/promote into the installed skill directory

## Relationship to FFmpeg work
This is a reusable ops skill, separate from the FFmpeg app and usable for any project database lifecycle.
