# core.ffmpeg portfolio roadmap

## 90-day evidence snapshot
- Commits (90 days): 7
- Files changed (90 days): 143
- Last signal: 26ca980 (4 days ago)
- Top modified areas: dotnet(79);docs(29);typescript(10);test(8);00_agile(6);.github(2)
- Notes: clean_at_scan

## Current repo posture
- Stack: .NET
- Docs folder: yes
- Roadmap folder: no
- Features docs: yes
- Tests indexed: yes

## Discovery
- [x] Capture and timestamp recent change signal
- [x] Capture top-level area concentration
- [x] Document `core.ffmpeg` as the reusable FFmpeg engine slice in the cross-repo media platform.
- [ ] Document owner and intent for area: dotnet(79)
- [ ] Add explicit release gates for next validation steps

## V1 (stability)
- [ ] Close gaps in docs and feature notes for recently touched areas
- [ ] Add or update smoke checks for changed source paths
- [ ] Validate packaging and deploy assumptions where infra/config changed
- [x] Add a Busey-Box-compatible Twitch/RTMP output helper.
- [x] Add a Busey-Box-compatible broadcaster command adapter.
- [x] Add a shared render preset application layer.
- [x] Add a shared active-preset resolver.
- [x] Add a shared encoder-resolution helper.
- [x] Add a resolved preset-aware pipeline helper.

## V2 (confidence)
- [ ] Add deeper tests on highest-churn areas
- [ ] Expand runbooks for recurring operator or publishing workflows
- [ ] Standardize naming and checklist structure for future items
- [x] Add a Twitch/RTMP example that matches Busey-Box's current Docker broadcaster command shape.
- [x] Decide how downstream apps should pass stream keys without making `core.ffmpeg` own secrets.
- [x] Port the preset/filter merge logic from `chop-it/v2`.
- [x] Port preset selection logic from `chop-it/v2`.
- [x] Port encoder fallback logic from `chop-it/v2`.
- [x] Port preset-aware pipeline resolution from `chop-it/v2`.

## V10 (scale)
- [ ] Move to a stable platform pattern with cross-repo checklist templates
- [ ] Split roadmap into discrete feature-level and initiative-level folders
- [ ] Define long-range acceptance criteria with operational and product owners

## Top touched files (90-day top 10)
- .codex
- .dockerignore
- .github/workflows/ci.yml
- .github/workflows/deploy.yml
- .gitignore
- ... and 5 more

## Follow-up ideas
- [ ] Convert area signals into one short feature roadmap within docs/features
- [ ] Add changelog notes in docs for behavior-impacting updates
- [ ] Add simple owner checklist for release readiness
- [x] Add a compatibility note for `app-busey-box/app/run_ffmpeg.sh`.

## Related platform docs

- [SvengoolieDB media metadata transition](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/README.md>)
- [One-swoop execution map](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/execution-map.md>)
- [Cross-project link map](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/link-map.md>)
