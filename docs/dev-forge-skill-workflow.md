# Dev-forge skill workflow

## Goal
Build reusable skills in a dedicated workspace, then copy/promote them into the installed skill location once they are ready.

## Recommended workspace
- source/dev workspace: `~/code/dev-forge/skills`
- installed runtime location: wherever OpenClaw expects the active skill folder

## Suggested folder layout

```text
~/code/dev-forge/skills/
  postgres-provisioner/
    SKILL.md
    scripts/
    references/
    tests/
```

## Workflow
1. Create the skill in the dev-forge workspace.
2. Iterate on docs, scripts, and tests there.
3. Validate the behavior locally.
4. Copy the finished skill into the installed skill location.
5. Version or tag the source copy in dev-forge for future updates.

## Why this works
- keeps experimentation out of the runtime skill path
- makes review easier
- allows multiple draft versions before promotion
- preserves a clear source-of-truth for the skill source

## Promotion checklist
- SKILL.md is complete
- scripts run cleanly
- references are current
- tests/examples are included
- the target install path is updated

## Good candidate skill categories
- provisioning helpers
- homelab operators
- container/database automation
- repo-specific workflows
