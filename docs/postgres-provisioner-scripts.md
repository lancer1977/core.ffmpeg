# PostgreSQL provisioner script set

## Goal
Provide a simple, predictable set of scripts for the PostgreSQL provisioner skill.

## Suggested scripts

### `create`
Create the database and user for a project.

```bash
create --project core-ffmpeg --env prod
```

### `rotate`
Rotate a database password and update the secret store.

```bash
rotate --project core-ffmpeg --env prod
```

### `drop`
Remove a project database/user pair.

```bash
drop --project core-ffmpeg --env test
```

### `status`
Report whether the DB and role exist.

```bash
status --project core-ffmpeg
```

### `list`
List known project databases and users managed by the tool.

```bash
list
```

## Implementation notes
- Prefer idempotent behavior.
- Print secrets only when explicitly asked.
- Use a dedicated admin/provisioning connection.
- Keep the scripts thin and composable.

## Suggested backend
- Bash wrappers around `psql`/`createdb`/`createuser`, or
- a small .NET CLI if you want richer error handling and secret integration
