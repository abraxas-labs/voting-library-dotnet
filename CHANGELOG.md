# ✨ Changelog (`v7.25.0`)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Version Info

```text
This version -------- v7.25.0
Previous version ---- v7.17.1
Initial version ----- v7.7.37
Total commits ------- 19
```

## [v7.25.0] - 2022-11-29

### 🔄 Changed

- add health check for event subscription failures

## [v7.24.0] - 2022-11-28

### 🔄 Changed

- add aggregate repository handling and event metadata read changes

## [v7.23.1] - 2022-11-23

### 🔄 Changed

- basis input validation

## [v7.23.0] - 2022-11-22

### 🆕 Added

- add grpc request logger tooling to record load testing playbooks

### 🔄 Changed

- introduce project wide variable for grpc packages to align package versions

## [v7.22.0] - 2022-11-21

### 🔄 Changed

- apply virtual keyword for repository public methods.

## [v7.21.1] - 2022-11-17

### 🔄 Changed

- make DOK Connector upload uri relative

## [v7.21.0] - 2022-11-16

### 🔄 Changed

- add TryGet method to export template repository
- move XML validation into Voting.Lib.Common

## [v7.20.5] - 2022-11-11

### 🔄 Changed

- ignore XML schema validation warnings in tests

## [v7.20.4] - 2022-11-10

### 🔄 Changed

- extend dm doc response models

## [v7.20.3] - 2022-11-08

### 🔄 Changed

- add testing utilities for XML and raw snapshots

## [v7.20.2] - 2022-11-07

### 🆕 Added

- add log messages for debugging
- add configure await to signal intention for continuation

## [v7.20.1] - 2022-10-20

### 🔄 Changed

- support default tenant id in authentication mock handler

## [v7.20.0] - 2022-10-20

### 🔄 Changed

- add option to expose exception message
- add option to expose exception message

## [v7.19.0] - 2022-10-20

### 🔄 Changed

- add option to set default tenant id

## [v7.18.1] - 2022-10-18

### 🔄 Changed

## [v7.18.0] - 2022-10-14

### 🔄 Changed

- add double and map proto validators

## [v7.17.2] - 2022-10-14

### 🔄 Changed

- Wabsti-C changes

## [v7.17.1] - 2022-09-27

### 🔄 Changed

- dmdoc update brick content

## [v7.17.0] - 2022-09-23

### 🔄 Changed

- add result bundle review templates

## [v7.16.1] - 2022-09-23

### 🔄 Changed

- relative brick content editor url

## [v7.16.0] - 2022-09-23

### 🔄 Changed

- dmdoc bricks

## [v7.15.0] - 2022-09-21

### 🔄 Changed

- add required eCH message type

## [v7.14.4] - 2022-09-20

### 🔄 Changed

- make UploadResponse constructor public

## [v7.14.3] - 2022-09-15

### 🔄 Changed

- adapt correlation id header to changed name for logging

## [v7.14.2] - 2022-09-06

### 🔄 Changed

- extend string rule validators

## [v7.14.1] - 2022-09-05

### 🔄 Changed

- print help and version text of CLI applications

## [v7.14.0] - 2022-09-05

### 🆕 Added

- add application builder extension which is adding the serilog request logging middleware enriching the log context with tracability properties

## [v7.13.1] - 2022-08-29

### 🔄 Changed

- proto string validator with trim check
- update readme

## [v7.13.0] - 2022-08-25

### 🔄 Changed

- proto validation with interceptors

## [v7.12.1] - 2022-08-23

### 🔄 Changed

- update dependencies and clean up code smells
- update changelog link
- add readme.md
- configure preview/public github urls and gpg key id for commit signing

## [v7.12.0] - 2022-08-08

### 🔄 Changed

- image processing

## [v7.11.3] - 2022-07-27

### 🆕 Added

- tenant header for modifications made by a service user

## [v7.11.2] - 2022-07-26

### 🆕 Added

- tenant header for modifications made by a service user
- powershell client generator script

### 🔄 Changed

- exclude parameters for successful generation

## [v7.11.1] - 2022-07-26

### 🔄 Changed

- validate subject of role token

## [v7.11.0] - 2022-07-13

### 🆕 Added

- CORS configuration support

## [v7.10.1] - 2022-06-10

### 🔄 Changed

- validate ssl errors if no certificate pin is available

## [v7.10.0] - 2022-06-09

### 🔄 Changed

- certificate pinning dangerously accept any certificate

## [v7.9.0] - 2022-06-02

### 🔄 Changed

- add bund and genf mocked test tenants
- add party votes and voter participation csv exports for proportional election unions

## [v7.8.0] - 2022-06-01

### 🔄 Changed

- add proportional election union party mandates csv report

## [v7.7.37] - 2022-06-01

### 🎉 Initial release for Bug Bounty
