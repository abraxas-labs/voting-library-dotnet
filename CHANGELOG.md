# ✨ Changelog (`v20.8.0`)

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Version Info

```text
This version -------- v20.8.0
Previous version ---- v20.5.0
Initial version ----- v7.7.37
Total commits ------- 4
```

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

### 🆕 Added

- add configurable timeout for dok connector calls

### 🔄 Changed

- return 503 service unavailable when readiness endpoint returns degraded

### 🆕 Added

- add configurability of property naming policy for dmDOC JSON data serialization

### 🆕 Added

- feat(VOTING-5993): add user notification attachements and zip files

BREAKING CHANGE: IFIle moved to common and renamed Filename to FileName.

- add configuration to allow token types
- by default ob-tokens are not accepted, unless explicitly allowed
- use single /token/roles/subject endpoint to resolve roles, remove configuration

BREAKING CHANGE: on behalf tokens are not accepted by default
BREAKING CHANGE: on behalf token endpoint is not configurable anymore

BREAKING CHANGE: `Page<T>` extends `PageInfo` now and all members except for the `Items` are moved to `PageInfo`. The ctor now accepts and exposes a list directly instead of the `IEnumerable`.

BREAKING CHANGE: removes unneeded pkcs#11 crypto provider config keys. If needed by the application, the app should subclass the config.

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

### 🆕 Added

- add HSM generic user type to support key management users

### Added

- added Ech0157v5 and Ech0159v5
