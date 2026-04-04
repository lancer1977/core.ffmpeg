# PostgreSQL provisioning CLI shape

## Goal
Provide a small operator CLI for creating and managing project databases/users, including test databases, without baking provisioning logic into the FFmpeg app.

## Suggested commands

### Create a project database
```bash
pg-provision create --project core-ffmpeg --env prod
```

Creates:
- database: `proj_core_ffmpeg`
- role: `proj_core_ffmpeg_app`
- optional admin role: `proj_core_ffmpeg_admin`

### Create a test database
```bash
pg-provision create --project core-ffmpeg --env test
```

Creates:
- database: `proj_core_ffmpeg_test`
- role: `proj_core_ffmpeg_test`

### Rotate credentials
```bash
pg-provision rotate --project core-ffmpeg --env prod
```

Rotates:
- user password
- any exported secret references

### Drop a project database
```bash
pg-provision drop --project core-ffmpeg --env test
```

Removes:
- database
- role
- grants associated with that environment

### Inspect status
```bash
pg-provision status --project core-ffmpeg
```

Shows:
- whether the database exists
- which roles exist
- whether credentials are current
- last migration or bootstrap status

## CLI inputs
- `--project <name>`
- `--env prod|test`
- `--db-name <name>` optional override
- `--user <name>` optional override
- `--password <secret>` or secret reference
- `--bootstrap-schema` optional migration bootstrap

## Expected behavior
- idempotent when possible
- safe to rerun
- emits clear output for automation
- avoids using superuser credentials unless absolutely required

## Secret handling
The CLI should:
- accept a secret reference when possible
- write credentials back to a secure store or deployment manifest
- never print passwords by default

## Implementation hint
The first version can be a thin wrapper around:
- `psql`/`createdb`/`createuser`
- or a direct PostgreSQL client library

## Relationship to core.ffmpeg
This tooling should stay separate from the FFmpeg host.
The app only needs runtime DB credentials; the provisioning CLI owns database lifecycle.
