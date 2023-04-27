# ✨ Changelog (`v10.12.5`)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Version Info

```text
This version -------- v10.12.5
Previous version ---- v7.26.10
Initial version ----- v7.7.37
Total commits ------- 30
```

## [v10.12.5] - 2023-04-03

### 🆕 Added

- add service name to service user mock data

## [v10.12.4] - 2023-03-30

### 🔄 Changed

- fixed encryption padding of aes mock

## [v10.12.3] - 2023-03-24

### 🔄 Changed

- replace eCH ballot question ID separator with underscore

## [v10.12.2] - 2023-03-22

### 🔄 Changed

- improve error message when DmDoc returns an unsuccessful response

## [v10.12.1] - 2023-03-14

### 🔄 Changed

- fix AesCbcEncryptionMock

## [v10.12.0] - 2023-03-13

### 🆕 Added

- Added ECDSA signature
- Added AES-MAC encryption

## [v10.11.1] - 2023-03-10

### 🔄 Changed

- truncate eCH delivery header values when they are too long

## [v10.11.0] - 2023-03-08

### 🔄 Changed

- eCH ballot question ID generator

## [v10.10.0] - 2023-02-24

### 🆕 Added

- Added bulk create signature
- Added more supported types in ByteConverter

## [v10.9.0] - 2023-02-23

### 🔄 Changed

- move method to match adjacent overload
- acquire async lock with timeout
- rest upload helpers

## [v10.8.0] - 2023-02-23

### 🔄 Changed

- input-validation-allow-more-character

## [v10.7.0] - 2023-02-22

### 🆕 Added

- add date only today value

## [v10.6.0] - 2023-02-20

### 🔄 Changed

- wm wahlergebnisse report

## [v10.5.1] - 2023-02-17

### 🔄 Changed

- wait until DmDoc draft is ready

## [v10.5.0] - 2023-02-16

### 🔄 Changed

- allow saving of an aggregate without idempotency guarantee

## [v10.4.0] - 2023-02-15

### 🔄 Changed

- ech 0045

## [v10.3.0] - 2023-02-15

### 🔄 Changed

- add transaction util

## [v10.2.0] - 2023-02-15

### 🔄 Changed

- string truncate extension

## [v10.1.0] - 2023-02-15

### 🔄 Changed

- add echserializer to perform streamed serialization of enumerable items

## [v10.0.0] - 2023-02-14

BREAKING CHANGE: added bulk root parameter

## [v9.0.0] - 2023-02-09

BREAKING CHANGE: export template models domain of influence types is now a readonly set

## [v8.1.0] - 2023-02-08

### 🆕 Added

- add overload for single file result creation to be independent of IFile

## [v8.0.1] - 2023-02-07

### 🔄 Changed

- Add assertion attribute so sonarqube recognizes the assertions

## [v8.0.0] - 2023-02-07

BREAKING CHANGE: changed DmDoc API because of better streaming support

## [v7.27.0] - 2023-01-30

### 🔄 Changed

- new export templates api

## [v7.26.10] - 2023-01-25

### 🔄 Changed

- add scoped dmdoc httpclient

## [v7.26.9] - 2023-01-25

### 🔄 Changed

- register dmdoc delegating handler

## [v7.26.8] - 2023-01-24

### 🔄 Changed

- add a constant key id in asymmetric algorithm mocks

## [v7.26.7] - 2023-01-24

### 🔄 Changed

- submit dmdoc username as part of the authentication header

## [v7.26.6] - 2023-01-09

### 🔄 Changed

- rename protocol description and filename

## [v7.26.5] - 2023-01-05

### 🔄 Changed

- add end result detail without empty and invalid votes protocol

## [v7.26.4] - 2022-12-21

### 🔄 Changed

- provide additional role debug infos

## [v7.26.3] - 2022-12-20

### 🔄 Changed

- improve dm doc bricks loading performance

## [v7.26.2] - 2022-12-14

### 🔄 Changed

## [v7.26.1] - 2022-12-12

### 🔄 Changed

- extend role token subject validation to support OBO subject tokens.

## [v7.26.0] - 2022-12-02

### 🆕 Added

- add headers to grpc request log output

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

### 🆕 Added

- add log messages for debugging
- add configure await to signal intention for continuation

### 🔄 Changed

- adapt correlation id header to changed name for logging

### 🆕 Added

- add application builder extension which is adding the serilog request logging middleware enriching the log context with tracability properties

### 🆕 Added

- tenant header for modifications made by a service user

### 🆕 Added

- tenant header for modifications made by a service user
- powershell client generator script

### 🔄 Changed

- exclude parameters for successful generation

### 🆕 Added

- CORS configuration support

these are for example eventstore internal events which are not interesting to voting at all

use the overload without any position, since the position is unsigned and always exclusive

BREAKING CHANGE: activity protocol by event store

BREAKING CHANGE: persistent subscription apis removed

BREAKING CHANGE: event signature

BREAKING CHANGE: removed user store dependency and used generated swagger clients

Replaces the userstore dependency with swagger generated clients

Eventstore should use the default http client builder to ensure certificate pinning
This removes ValidateCertificate from the event store config. This should now be configured via the cert pinning config

BREAKING CHANGE: VOTING-638 net6.0 update

note: not releasing as breaking change since all consumers are currently still on 1.x.x and the breaking change is in an api which is not indented for public use

BREAKING CHANGE: dotnet 5

also improve waiting for event store connection

also fixed aggregate repo

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

## [v7.13.0] - 2022-08-25

### 🔄 Changed

- proto validation with interceptors

## [v7.12.1] - 2022-08-23

### 🔄 Changed

- update dependencies and clean up code smells

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
