# PostgreSQL provisioning design

## Goal
Provide a repeatable way to create project databases, users, and test databases without mixing that logic into the FFmpeg application itself.

## Scope
This is an ops/provisioning layer for:
- project databases
- application users
- test databases
- least-privilege grants
- secret handoff into app/container deployment

## Recommended naming convention

### Databases
- `proj_<name>` for application data
- `proj_<name>_test` for test/staging data

### Users/Roles
- `proj_<name>_app`
- `proj_<name>_test`
- optionally a `proj_<name>_admin` role for migrations and provisioning

## Role model

### App role
- read/write only to its own database
- no superuser privileges
- no global CREATE DATABASE rights

### Test role
- limited to test DB
- can be recreated freely
- safe to rotate frequently

### Admin/provisioner role
- used by automation only
- creates databases, schemas, users, and grants
- not used by the application runtime

## Provisioning flow
1. Ensure the admin/provisioner connection exists in secrets.
2. Create the database if missing.
3. Create the role/user if missing.
4. Set/reset the password.
5. Grant ownership or scoped privileges.
6. Create a test database/user if requested.
7. Emit credentials into the secret store or deployment manifest.

## Safety guidance
- Prefer a dedicated provisioning role over reusing the app role.
- Never expose the provisioning credentials to the app container.
- Keep app runtime credentials scoped to a single database.
- For test projects, allow easy teardown and recreation.

## Homelab deployment pattern
- Postgres container or managed host service
- provisioning script/tool on the admin machine
- app container receives only runtime credentials
- reverse proxy and auth separate from DB access

## Suggested automation shape
A small CLI or service should accept:
- project name
- environment (prod/test)
- requested DB name
- requested user name
- password or secret reference
- optional schema migration bootstrap

## OpenClaw direction
This can become a reusable operator workflow so you can say:
- create DB/user for project X
- create test DB/user for project X
- rotate credentials
- remove project DBs cleanly

## Relationship to core.ffmpeg
The FFmpeg app should only care that a database exists for its own state.
The provisioning workflow should live outside the app so it can be reused for future projects too.
