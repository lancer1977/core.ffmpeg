# PostgreSQL Provisioner Skill - fuller spec

## Purpose
Provision and manage PostgreSQL resources for projects and test projects.

## Core responsibilities
- create databases
- create users/roles
- grant scoped permissions
- rotate credentials
- delete resources safely
- report status

## Inputs
- project name
- environment: prod/test
- db name override
- user name override
- password or secret reference
- bootstrap flag

## Outputs
- database name
- user name
- status of resources
- secret reference or credential update action

## Safety
- do not expose admin credentials in runtime output
- require confirmation for delete/drop
- use least privilege
- avoid global admin grants

## Workflow
1. create
2. verify status
3. rotate as needed
4. drop with confirmation

## Notes
This skill is intentionally separate from application runtime code.
