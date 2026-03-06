# ✨ Changelog (`v20.14.0`)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Version Info

```text
This version -------- v20.14.0
Previous version ---- v20.12.3
Initial version ----- v7.7.37
Total commits ------- 7
```

## [v20.14.0] - 2026-03-04

### 🔄 Changed

- event retry policy virtual

## [v20.13.3] - 2026-02-26

### 🔄 Changed

- make HSM PKCS#11 simulator registry path configurable via environment variable for integration tests

## [v20.13.2] - 2026-02-26

### 🔄 Changed

- handle grpc exceptions with status code as warning

## [v20.13.1] - 2026-02-25

### :lock: Security

- update magick image processing library to latest non vulnerable version

## [v20.13.0] - 2026-02-24

### :new: Added

- add parameter to pass async job priority for start async pdf generation
- set priorty for bundle review protocols to 10

## [v20.12.5] - 2026-02-23

### 🔄 Changed

- reduce log noise info into debug

## [v20.12.4] - 2026-02-20

### 🔄 Changed

- reduce loglevel to warning

## [v20.12.3] - 2026-02-06

### 🔄 Changed

- extend CD pipeline with enhanced bug bounty publication workflow

## [v20.12.2] - 2026-02-03

### 🔄 Changed

- fix eCH-0045 v6 delivery header serializer info

## [v20.12.1] - 2026-01-27

### 🔄 Changed

- never add tokenhandler to DI as these should always be resolved via factory

## [v20.12.0] - 2026-01-15

### 🔄 Changed

- add random https url string util

## [v20.11.0] - 2026-01-15

### 🔄 Changed

- add https url string validation

## [v20.10.0] - 2026-01-08

### 🔄 Changed

- add kms ecdsa crypto operations

## [v20.9.0] - 2026-01-08

### 🔄 Changed

- time zone constants

## [v20.8.2] - 2025-12-10

### 🔄 Changed

- fix update ignore relations method

## [v20.8.1] - 2025-11-05

### 🔄 Changed

- add eCH-0072 scheme

## [v20.8.0] - 2025-10-15

### 🆕 Added

- add configurable timeout for dok connector calls

## [v20.7.1] - 2025-10-14

### 🔄 Changed

- allow plus-minus sign in complex text validation ruleset

## [v20.7.0] - 2025-10-06

### 🔄 Changed

- update eCH-0252-2 to most recent version

## [v20.6.0] - 2025-10-01

### 🔄 Changed

- add eCH-0157 and 0159 v5.1 export templates

## [v20.5.0] - 2025-10-01

### 🔄 Changed

- base64url encoding and url security tokens

## [v20.4.0] - 2025-09-30

### 🔄 Changed

- return 503 service unavailable when readiness endpoint returns degraded

### 🆕 Added

- add configurability of property naming policy for dmDOC JSON data serialization

## [v20.3.1] - 2025-09-25

### 🔄 Changed

- expose build status method from grpc exception interceptor

## [v20.3.0] - 2025-09-25

### 🔄 Changed

- add eCH-0045 v6

## [v20.2.0] - 2025-09-25

### 🔄 Changed

- add piped file

## [v20.1.1] - 2025-09-24

### 🔄 Changed

- extend complex string validator to support square brackets

## [v20.1.0] - 2025-09-24

### 🔄 Changed

- expose user email

## [v20.0.0] - 2025-09-23

### 🆕 Added

- feat(VOTING-5993): add user notification attachements and zip files

BREAKING CHANGE: IFIle moved to common and renamed Filename to FileName.

## [v19.3.0] - 2025-09-22

### 🔄 Changed

- 2fa nevis communication

## [v19.2.0] - 2025-09-17

### 🔄 Changed

- add kms bulk create hmac and bulk encrypt

## [v19.1.0] - 2025-09-09

### 🔄 Changed

- kms integration tests

## [v19.0.2] - 2025-08-27

### 🔄 Changed

- fix ImageMagick vulnerability (CVE-2025-53015)

## [v19.0.1] - 2025-08-14

### 🔄 Changed

- switching malware scanner to https without proxy interceptor

## [v19.0.0] - 2025-08-13

- add configuration to allow token types
- by default ob-tokens are not accepted, unless explicitly allowed
- use single /token/roles/subject endpoint to resolve roles, remove configuration

BREAKING CHANGE: on behalf tokens are not accepted by default
BREAKING CHANGE: on behalf token endpoint is not configurable anymore

## [v18.2.0] - 2025-08-08

### 🔄 Changed

- add eCH ABX-VOTING 1-5

## [v18.1.2] - 2025-08-06

### 🔄 Changed

- validate that onbehalf options resource is set

## [v18.1.1] - 2025-08-05

### 🔄 Changed

- check subject on returned role token

## [v18.1.0] - 2025-07-15

### 🔄 Changed

- add on behalf token handling

## [v18.0.0] - 2025-07-09

BREAKING CHANGE: `Page<T>` extends `PageInfo` now and all members except for the `Items` are moved to `PageInfo`. The ctor now accepts and exposes a list directly instead of the `IEnumerable`.

## [v17.2.1] - 2025-07-07

### 🔄 Changed

- improve kms exception content

## [v17.2.0] - 2025-07-03

### 🔄 Changed

- add basis eCH contest e-voting only export

## [v17.1.0] - 2025-07-02

### 🔄 Changed

- add eCH-0252 proportional election with info export

## [v17.0.0] - 2025-06-30

BREAKING CHANGE: removes unneeded pkcs#11 crypto provider config keys. If needed by the application, the app should subclass the config.

## [v16.2.1] - 2025-06-27

### 🔄 Changed

- bump pkcs11 driver from 4.45 to 4.51.0.1

## [v16.2.0] - 2025-06-27

### 🔄 Changed

- crypto mock: support private public keypairs

## [v16.1.0] - 2025-06-26

### 🔄 Changed

- add key labels to kms client

## [v16.0.0] - 2025-06-25

### 🆕 Added

- feat(VOTING-5934): implement kms

Breaking Change:
- this abstracts crypto operations into new abstractions
- the PKCS11 implementation is moved to `Voting.Lib.Cryptography.Pkcs11`
- the `IHsmDeviceAdapter` is renamed to `ICryptoProvider`
- all methods of `ICryptoProvider` now expect the `string keyId` as a last parameter. This is the CKA label for PKCS#11.
- all methods of `ICryptoProvider` are now async.
- The key is removed from the config to better separate it.
- `GenerateGenericSecretKey` is renamed to `GenerateMacSecretKey`
- `DeleteSecretKey` is split up into `DeleteMacSecretKey` and `DeleteAesSecretKey`
- `AddPkcs11HealthCheck` is renamed to `AddCryptoProviderHealthCheck`
- The name of the health check is adjusted from `Pkcs11` to `CryptoProvider`. The first parameter of `AddCryptoProviderHealthCheck` can be used to use the old name.

BREAKING CHANGE: pkcs11 implementation is moved from Voting.Lib.Cryptography to Voting.Lib.Cryptography.Pkcs11

## [v15.13.1] - 2025-06-06

### 🔄 Changed

- extend complex single-/multiline validator with paragraph sign (§)

## [v15.13.0] - 2025-06-06

### 🆕 Added

- add HSM generic user type to support key management users

## [v15.12.0] - 2025-06-04

### 🔄 Changed

- hsm mock: derive key from key label

## [v15.11.0] - 2025-05-28

### 🔄 Changed

- add for update ef core helper

## [v15.10.0] - 2025-05-26

### 🔄 Changed

- support oneof in proto validators

## [v15.9.0] - 2025-05-26

### 🔄 Changed

- add long validator

## [v15.8.0] - 2025-05-23

### 🔄 Changed

- add separate service token handling

## [v15.7.0] - 2025-05-23

### Added

- added Ech0157v5 and Ech0159v5

## [v15.6.0] - 2025-05-22

### 🔄 Changed

- add timeout to secure connect options

## [v15.5.0] - 2025-05-22

### 🔄 Changed

- add additional http headers option

## [v15.4.1] - 2025-05-21

### 🔄 Changed

- allow " in complex ml texts

## [v15.4.0] - 2025-05-20

### 🔄 Changed

- add hsm secret key management and hmac sha support

## [v15.3.0] - 2025-05-15

### 🔄 Changed

- add random string util

## [v15.2.2] - 2025-05-14

### 🔄 Changed

- add secondary election end result detail protocols

## [v15.2.1] - 2025-05-12

### 🔄 Changed

- add tab character to complex text validation

## [v15.2.0] - 2025-05-05

### 🔄 Changed

- extract validation regexes into separate project

## [v15.1.0] - 2025-04-30

### 🔄 Changed

- add smtp user notifications sender
- Implements optimistic locking for PostgreSQL.

## [v15.0.3] - 2025-04-04

### 🔄 Changed

- add byte[] method to malware scanner

## [v15.0.2] - 2025-04-02

### 🔄 Changed

- compare lists by identifier and not by order

## [v15.0.1] - 2025-04-02

### 🔒 Security

- harden regex to prevent potential DoS vulnerability caused by super-linear runtime due to excessive backtracking.

## [v15.0.0] - 2025-03-20

BREAKING CHANGE: AddMockedTimeProvider is renamed to AddMockedClock to align regular method names.

## [v14.0.3] - 2025-03-17

### 🔄 Changed

- update eCH-0252 schema in eCH-0252 project

## [v14.0.2] - 2025-03-17

### 🔄 Changed

- update eCH-0252 version

## [v14.0.1] - 2025-03-17

### 🔄 Changed

- update eCH-0252 version

## [v14.0.0] - 2025-03-11

BREAKING CHANGE: event serializer public api changes, see previous commit

## [v13.4.0] - 2025-03-06

### 🔄 Changed

- add PerDomainOfInfluence to report template model

## [v13.3.0] - 2025-03-06

### 🔄 Changed

- move getconsumers method to hub for backward compatibility

## [v13.2.0] - 2025-03-05

### 🔄 Changed

- add event processing context

## [v13.1.0] - 2025-02-27

### 🔄 Changed

- improved messaging

## [v13.0.0] - 2025-02-27

BREAKING CHANGE: Renamed Abx_Voting_1_0 to ABX_Voting_1_0 to match casing of other generated classes

## [v12.25.0] - 2025-02-12

### 🔄 Changed

- add XmlValidationOnWriteStream

## [v12.24.1] - 2025-01-28

### 🔄 Changed

- introduce a max command text length configuration for db query monitoring
- truncate command text for logging when limit exeeded

## [v12.24.0] - 2025-01-28

### 🆕 Added

- add person extension for householder in ech-0045

## [v12.23.0] - 2025-01-20

### 🆕 Added

- add HSM integration tests based on the cryptoserver simulator docker image.
- add integration tests for all cryptographic operations provided by the HSM adapter.

### ❌ Removed

- remove public key retrieval in HSM health check to decouple from specific application use cases.

## [v12.22.3] - 2025-01-10

### 🔄 Changed

- update Pkcs11Interop library from 5.1.2 to 5.2.0.
- refer to GitHub release notes [Pkcs11Interop 5.2.0](https://github.com/Pkcs11Interop/Pkcs11Interop/releases/tag/v5.2.0)

## [v12.22.2] - 2024-12-20

### 🔄 Changed

- extend dmdoc lib for selection of active bricks

## [v12.22.1] - 2024-12-19

### 🔄 Changed

- add x-vrsg-tenant query parameter to be able to update a user

## [v12.22.0] - 2024-12-18

### 🔄 Changed

- update minio lib and testcontainer according to latest operated version

## [v12.21.0] - 2024-12-16

### 🔄 Changed

- update eCH-0252-2-0 version

## [v12.20.0] - 2024-12-13

### 🆕 Added

- introduced a setting which allows to include the user id in the log output

## [v12.19.1] - 2024-12-12

### 🔄 Changed

- enable e-voting reports for municipality political businesses

## [v12.19.0] - 2024-12-12

### 🔄 Changed

- snapshotting of bricks

## [v12.18.0] - 2024-12-09

### 🔄 Changed

- fix flaky tests by using fake time provider

## [v12.17.2] - 2024-12-08

### 🔄 Changed

- upgrade test containers and use minio provided container, fixes flaky wait condition

## [v12.17.1] - 2024-12-04

### 🔄 Changed

- add end result detail protocol for multiple counting circle results

## [v12.17.0] - 2024-12-03

### 🔄 Changed

- add secondary majority election protocols

## [v12.16.3] - 2024-12-03

### 🔄 Changed

- remove vote temporary end result protocol

## [v12.16.2] - 2024-11-28

### 🔄 Changed

- support publish large amount of events

## [v12.16.1] - 2024-11-27

### 🔄 Changed

- disable e-voting reports for municipality

## [v12.16.0] - 2024-11-08

### 🔄 Changed

- add event sourcing read stream version api

## [v12.15.5] - 2024-10-30

### :arrows_counterclockwise: Changed

- update xscgen and add serializeEmptyCollections option

## [v12.15.4] - 2024-10-23

### 🔄 Changed

- init eventing meter histogram to avoid no data alerts

### 🔒 Security

- update Microsoft.Extensions.Caching.Memory to close vulnerability

## [v12.15.3] - 2024-10-08

### 🔄 Changed

- remove domain of influence types for result protocol and counting circle protocol

## [v12.15.2] - 2024-10-08

### 🔄 Changed

- eCH-0045 delivery header deserializer

## [v12.15.1] - 2024-09-27

### ⚠️ Deprecated

- remove deprecated second factor code, instead use jwts as primary codes

## [v12.15.0] - 2024-09-26

### 🔄 Changed

- add comment to delivery header provider

## [v12.14.0] - 2024-09-10

### 🔄 Changed

- consider testing phase in testDeliveryFlag

## [v12.13.0] - 2024-09-05

### 🔄 Changed

- add new eCH-0252 2.0 version

## [v12.12.1] - 2024-08-27

### :arrows_counterclockwise: Changed

- update bug bounty template reference
- patch ci-cd template version, align with new defaults

## [v12.12.0] - 2024-08-22

### 🔄 Changed

- add db performance utils

## [v12.11.0] - 2024-08-20

### 🔄 Changed

- add 2fa fallback qr code

## [v12.10.9] - 2024-08-19

### 🔄 Changed

- ensure swagger generator can be disabled completely

## [v12.10.8] - 2024-08-14

### 🔄 Changed

- add eCH-0252 info templates to ausmittlung export

## [v12.10.7] - 2024-08-13

### 🔄 Changed
- enable continuous integration build property for dotnet CLI
- preserve source revision information version to enable source-link feature

## [v12.10.6] - 2024-08-12

### 🔄 Changed

- prevent source revision from being included in release builds to preserve deterministic builds.

## [v12.10.5] - 2024-08-07

Refactored StringValidator to consolidate validation methods into a single `ValidateString` method, now handling various string validations. Enhanced `ValidateString` to check for untrimmed whitespace and report invalid characters in UTF-8 hex format. Added `GetNonMatchingCharactersAsUtf8Hex` method for UTF-8 hex conversion. Updated `ProtoValidatorTest` and `StringValidatorTest` to reflect new error messages and include additional test cases. Renamed and refactored test methods for clarity.

## [v12.10.4] - 2024-08-06

### 🔄 Changed

- extend multipart file with content type and assertion.

## [v12.10.3] - 2024-07-31

### 🔄 Changed

- speed up database deletion methods

## [v12.10.2] - 2024-07-05

### 🔄 Changed

- Extend multipart data with content type to enable validation of section-specific content types for consumers.

## [v12.10.1] - 2024-07-04

### 🔄 Changed

- compare http headers with case-insensitivity according to rfc-2616

## [v12.10.0] - 2024-06-25

### 🔄 Changed

- create zip file with last time the entry changed

## [v12.9.2] - 2024-06-20

### 🔄 Changed

- allow template vote_result_e_voting_only for Municipality

## [v12.9.1] - 2024-05-28

### 🔄 Changed

- split ech-0252 election to majority and proportional election export

## [v12.9.0] - 2024-05-16

### 🔄 Changed

- priority execution of preview printjobs

## [v12.8.3] - 2024-05-15

### 🔄 Changed

- allow equal sign for complex fields

## [v12.8.2] - 2024-05-13

### 🔄 Changed

- allow left single quotation mark for complex input fields

## [v12.8.1] - 2024-05-08

### 🔄 Changed

- use different file name for eCH-0252 vote and election exports

## [v12.8.0] - 2024-05-07

### 🔄 Changed

- add eCH-0252 for elections

## [v12.7.1] - 2024-05-06

### 🔄 Changed

- rename protocol export name

## [v12.7.0] - 2024-04-25

### 🔄 Changed

- double proportional election protocol

## [v12.6.3] - 2024-04-22

### 🔄 Changed

- deserialize whole category tree with children

## [v12.6.2] - 2024-04-16

### 🔄 Changed

- remove temporary tenant for 2fa transaction confirmation authorization
- updated VOTING IAM API client

## [v12.6.1] - 2024-04-04

### 🔄 Changed

- add parameter to proportional election candidate results with vote sources template filename

## [v12.6.0] - 2024-03-27

### 🔄 Changed

- add eCH-0252 export

## [v12.5.0] - 2024-03-25

### 🔄 Changed

- decimal extensions

## [v12.4.0] - 2024-03-20

### 🔄 Changed

- add double proportional export templates

## [v12.3.0] - 2024-03-12

### 🔄 Changed

- add wp listen sk stat export

## [v12.2.2] - 2024-03-12

### 🔄 Changed

- add percent sign for complex single- and multiline string validation

## [v12.2.1] - 2024-03-01

### 🔒 Security

- remove deprecated dependency Microsoft.Extensions.PlatformAbstractions

## [v12.2.0] - 2024-02-22

### 🔄 Changed

- add list votes end result union export

## [v12.1.0] - 2024-02-16

### 🔄 Changed

- add proportional wabsti exports with a single political business

## [v12.0.3] - 2024-02-15

### 🔄 Changed

- prevent malware scanner from closing input stream
- ensure DLL import resolver is only registered once

## [v12.0.2] - 2024-02-14

### 🔄 Changed

- add support for apostophe mark for complex single- and multiline string validation

## [v12.0.1] - 2024-02-14

### 🆕 Added

- register custom dll resolver for pkcs11 adapter

## [v12.0.0] - 2024-02-12

### 🔄 Changed

- Update from .NET 6 to .NET 8

### 🔒 Security

- Apply patch policy for outdated and vulnerable dependencies. The components EventStore, Minio and MassTransit are patched as part of a follow-up task.

## [v11.33.1] - 2024-02-08

### 🔄 Changed

- move mock to lib and add tests

## [v11.33.0] - 2024-02-02

### 🆕 Added

- support for database query monitoring

## [v11.32.0] - 2024-01-31

### 🔄 Changed

- add AuthorizeAnyPermission to authorize against a list of permissions

## [v11.31.0] - 2024-01-29

### 🔄 Changed

- add wp gemeinden sk stat export

## [v11.30.1] - 2024-01-22

### 🔄 Changed

- integrate api client generator for dotnet as self-managed project dependency in voting libraries.

## [v11.30.0] - 2024-01-05

### 🔄 Changed

- add eCH-0252

## [v11.29.0] - 2023-12-20

### 🔄 Changed

- add permissions to Voting.Lib.Iam

## [v11.28.0] - 2023-12-19

### 🔄 Changed

- add eCH generator

## [v11.27.1] - 2023-12-15

### 🔄 Changed

- typo in proto validation max value message

## [v11.27.0] - 2023-11-29

### 🔄 Changed

- add DocPipe

## [v11.26.1] - 2023-11-17

### 🔄 Changed

- roles cache should not store empty roles

## [v11.26.0] - 2023-11-14

### 🔄 Changed

- add csv vote e-voting details export

## [v11.25.0] - 2023-11-10

### 🆕 Added

- add dmdoc callback fail policy
- add dmdoc callback timeout parameter
- add documentation

## [v11.24.0] - 2023-11-09

### 🔄 Changed

- add vote e-voting protocols

## [v11.23.0] - 2023-11-09

### 🔄 Changed

- add e-voting vote result protocol

## [v11.22.0] - 2023-11-08

### 🆕 Added

- add dmdoc draft cleanup mode to handle cleanup requirements
- separate dequeuing and processing of queued items and delay execution

## [v11.21.0] - 2023-11-07

### 🆕 Added

- add dmdoc draft cleanup job
- add dmdoc draft cleanup queue
- add scheduler config and DI registration for the job and queue

## [v11.20.0] - 2023-11-07

### 🆕 Added

- add dmdoc draft deletion options for hard deletion and content only

## [v11.19.1] - 2023-11-07

### 🔄 Changed

- migrate from deprecated DotNet.Testcontainers to Testcontainers

## [v11.19.0] - 2023-10-31

### 🔄 Changed

- add dmdoc callback retry policy and generator version

## [v11.18.3] - 2023-10-23

### 🔄 Changed

- upgrade magick.net

## [v11.18.2] - 2023-10-22

### 🔄 Changed

- role token cache key based on subject instead of access token

## [v11.18.1] - 2023-10-19

### 🔄 Changed

- map correct is low prio health check

## [v11.18.0] - 2023-10-18

### 🔄 Changed

- add detailed CSV activity protocol

## [v11.17.1] - 2023-10-11

### 🔄 Changed

- use correct event position in eventing meter

## [v11.17.0] - 2023-10-10

### 🆕 Added

- add roles cache to minimize calls to iam

## [v11.16.0] - 2023-10-09

### 🔄 Changed

- ahvN13 calculate checksum

## [v11.15.0] - 2023-10-04

### 🔄 Changed

- add meters for duration and count of event processing by event type

## [v11.14.1] - 2023-09-25

### 🔄 Changed

- inject malware scanner config

## [v11.14.0] - 2023-09-25

### 🔄 Changed

- add wp gemeinden bfs export

## [v11.13.0] - 2023-09-21

### 🔄 Changed

- eVoting protocols

## [v11.12.0] - 2023-08-31

### 🆕 Added

- config option to disable malware scanning

### 🔄 Changed

- skip malware scanning request if it is disabled by config

## [v11.11.0] - 2023-08-28

### 🆕 Added

- add http probes health check for 3rd party apis

### 🔄 Changed

- extend cert pinning config with http probe health check option

## [v11.10.0] - 2023-08-25

### 🔄 Changed

- enhance logging for cert pinning

## [v11.9.1] - 2023-08-25

### 🔄 Changed

- use stream name instead of type in aggregate mock store

## [v11.9.0] - 2023-08-25

also store aggregate type in aggregate store mock

## [v11.8.1] - 2023-08-24

### 🔄 Changed

- expand input validation

## [v11.8.0] - 2023-08-23

### 🔄 Changed

- allow generic context type for list comparison

## [v11.7.0] - 2023-08-22

### 🔄 Changed

- update eai and validation proto dependency to deterministic version

## [v11.6.1] - 2023-08-22

### 🔄 Changed

- add form field names to MultipartRequestHelper

## [v11.6.0] - 2023-08-22

### 🔄 Changed

- add ability to handle multiple files to MultipartRequestHelper

## [v11.5.0] - 2023-08-15

### 🆕 Added

- add user secret support to configuration builder

## [v11.4.0] - 2023-08-15

### 🆕 Added

- add iam app handler to pass the configured app header with every request

## [v11.3.0] - 2023-08-15

### 🆕 Added

- add grpc path prefix delegation handler

## [v11.2.0] - 2023-08-15

### 🆕 Added

- add junit test reporting services

## [v11.1.1] - 2023-08-10

### 🔄 Changed

- add eCH-0157 extension for candidate additions.

## [v11.1.0] - 2023-08-09

### 🔄 Changed

- deterministic build and include sha256 of binary into nuget package

## [v11.0.0] - 2023-07-25

BREAKING CHANGE: This changes the hash / byte array if guids are involved. The old behaviour can still be applied if Guid.ToString() is used instead

## [v10.29.0] - 2023-07-12

### 🔄 Changed

- cron jobs

## [v10.28.0] - 2023-07-11

### 🔄 Changed

- abort the request if the response has already started
- !(VOTING-3367): add option to keep the writer open when writing eCH async enumerable data

## [v10.27.1] - 2023-07-05

### 🔄 Changed

- use pkcs11 interop factory for automated target platform high level api discovery

## [v10.27.0] - 2023-07-05

### 🆕 Added

- extended malware scanner service for string content

## [v10.26.1] - 2023-07-03

### 🔄 Changed

- Change default AES algorithm from CBC to GCM. Refine asymmetric and symmetric naming conventions.

## [v10.26.0] - 2023-06-30

### 🆕 Added

- malware scanner service

## [v10.25.0] - 2023-06-28

### 🔄 Changed

- age utility to calculate age

## [v10.24.0] - 2023-06-27

### 🔄 Changed

- ICollection to IReadOnlyCollection support

## [v10.23.1] - 2023-06-26

### 🔄 Changed

- ignore casing when comparing hex public keys of certificate pins

## [v10.23.0] - 2023-06-21

### 🔄 Changed

- add Parse method to Ahvn13

## [v10.22.1] - 2023-06-21

### 🔄 Changed

- extend byte converter and hash builder with delimited operations.

## [v10.22.0] - 2023-06-15

### 🔄 Changed

- add hash builder pool

## [v10.21.0] - 2023-06-13

### 🔄 Changed

- nullable support for byte converter and hash builder

## [v10.20.0] - 2023-06-13

### 🔄 Changed

- hash builder and imporved performance for byte converter

## [v10.19.1] - 2023-06-13

### 🔄 Changed

- extend complex text input validation rule, escape characters

## [v10.19.0] - 2023-06-12

### 🔄 Changed

- add AHVN13

## [v10.18.0] - 2023-06-06

### 🔄 Changed

- hsm public key extraction api

## [v10.17.0] - 2023-05-23

### 🔄 Changed

- add header propagation

## [v10.16.1] - 2023-05-17

### 🔄 Changed

- action id with multiple aggregates

## [v10.16.0] - 2023-05-15

### 🔄 Changed

- add mock service collection extension helper

## [v10.15.0] - 2023-05-15

### 🔄 Changed

- add sg abstimmungsergebnisse report

## [v10.14.0] - 2023-05-11

### 🔄 Changed

- add gRPC http handler extensions, add auth store access token, add pass through grpc auth

## [v10.13.1] - 2023-05-10

### 🔄 Changed

- mark transaction scope as obsolete

## [v10.13.0] - 2023-05-03

### 🔄 Changed

- add ech0045 swiss extension

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

- extend complex text input validation rules (dash)

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

- Extend complex single and multiline string validators with character '/' (slash)

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
