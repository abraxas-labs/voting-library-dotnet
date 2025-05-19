# ✨ Changelog (`v15.3.0`)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Version Info

```text
This version -------- v15.3.0
Previous version ---- v12.24.0
Initial version ----- v7.7.37
Total commits ------- 23
```

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

- compare lists by identifier and not by order

### 🔒 Security

- harden regex to prevent potential DoS vulnerability caused by super-linear runtime due to excessive backtracking.

BREAKING CHANGE: AddMockedTimeProvider is renamed to AddMockedClock to align regular method names.

BREAKING CHANGE: event serializer public api changes, see previous commit

BREAKING CHANGE: Renamed Abx_Voting_1_0 to ABX_Voting_1_0 to match casing of other generated classes

### 🔄 Changed

- introduce a max command text length configuration for db query monitoring
- truncate command text for logging when limit exeeded

### 🆕 Added

- add person extension for householder in ech-0045

### 🆕 Added

- add HSM integration tests based on the cryptoserver simulator docker image.
- add integration tests for all cryptographic operations provided by the HSM adapter.

### ❌ Removed

- remove public key retrieval in HSM health check to decouple from specific application use cases.

### 🔄 Changed

- update Pkcs11Interop library from 5.1.2 to 5.2.0.
- refer to GitHub release notes [Pkcs11Interop 5.2.0](https://github.com/Pkcs11Interop/Pkcs11Interop/releases/tag/v5.2.0)

### 🔄 Changed

- update minio lib and testcontainer according to latest operated version

### 🆕 Added

- introduced a setting which allows to include the user id in the log output

### :arrows_counterclockwise: Changed

- update xscgen and add serializeEmptyCollections option

### 🔄 Changed

- init eventing meter histogram to avoid no data alerts

### 🔒 Security

- update Microsoft.Extensions.Caching.Memory to close vulnerability

### ⚠️ Deprecated

- remove deprecated second factor code, instead use jwts as primary codes
