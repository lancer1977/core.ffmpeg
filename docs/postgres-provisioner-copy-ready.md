# PostgreSQL provisioner copy-ready package

## Source of truth
Build in:
`~/code/dev-forge/skills/postgres-provisioner`

## Final install target
Whatever OpenClaw expects for installed skills in the runtime environment.

## Package contents
- `SKILL.md`
- `scripts/`
- `references/`
- `tests/`

## Suggested promote command
```bash
cp -R ~/code/dev-forge/skills/postgres-provisioner <installed-skill-path>/postgres-provisioner
```

## Check before copy
- skill content complete
- scripts executable
- references current
- tests or smoke checks included
- no draft-only notes left behind
