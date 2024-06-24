# GoogleAPI Plugin for Stream Dock

![GitHub last commit](https://img.shields.io/github/last-commit/DevAnyKR/StreamDock.Plugin.GoogleAPI)
![GitHub License](https://img.shields.io/github/license/devanykr/StreamDock.Plugin.GoogleAPI)
![GitHub Discussions](https://img.shields.io/github/discussions/devanykr/StreamDock.Plugin.GoogleAPI)
![GitHub repo size](https://img.shields.io/github/repo-size/devanykr/StreamDock.Plugin.GoogleAPI)
![GitHub Repo stars](https://img.shields.io/github/stars/devanykr/StreamDock.Plugin.GoogleAPI?style=plastic&label=%E2%AD%90)

![GitHub Release Date](https://img.shields.io/github/release-date/devanykr/StreamDock.Plugin.GoogleAPI)
![GitHub Release](https://img.shields.io/github/v/release/devanykr/StreamDock.Plugin.GoogleAPI)
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/devanykr/StreamDock.Plugin.GoogleAPI/total)

스트림독에서 구글 API 기능을 사용할 수 있습니다. 보안이 우수한 OAuth 2.0으로 구글 계정을 인증합니다.

## Preview

![image](https://github.com/DevAnyKR/StreamDock.Plugin.GoogleAPI/assets/110871727/e156bae5-c4bd-4de1-830a-45f8ee3842ee)

![image](https://github.com/DevAnyKR/StreamDock.Plugin.GoogleAPI/assets/110871727/4ce2d45f-a60a-47ee-b79b-bce1fbf74de0)

## Hardware, Original

➡️ [AliExpress Store](https://s.click.aliexpress.com/e/_DC8mx5N)

![image](https://github.com/DevAnyKR/StreamDock.Plugin.GoogleAPI/assets/110871727/a5a3d159-9bee-4287-a0ee-ed2abd64cf6d)

Stream Dock 100% Compatible Brands: MiraBox, AJAZZ, Monstar Deck

## How to Install...

0. 스트림독을 종료합니다.
1. `Visual Studio`에서 컴파일하거나 릴리스 파일을 내려받아 압축을 풉니다.
2. `kr.devany.googleapi.adsense.sdPlugin` 폴더를 `%AppData%\HotSpot\StreamDock\plugins\` 위치에 복사합니다.
3. 스트림독을 실행합니다.

## Conditions (서비스 제공자용)

- 서비스 제공자는 필요한 기능의 [Google API](https://console.cloud.google.com/) 키를 발급받아야 합니다. Google Cloud에서 만들 수 있습니다.
- 원본과 다른 이름으로 프로젝트 서비스를 하려면 사용자 인증 정보(OAuth 2.0 클라이언트 ID)를 만들어야 합니다.

 ### Google API, OAuth 2.0

1. 구글 콘솔에 접속합니다. [Google Console](https://console.cloud.google.com/) 
2. 새 프로젝트를 생성합니다.
3. 사용하려는 API 키를 생성합니다.
    * `AdSense Management` 외 필요한 API
4. OAuth Client ID를 생성합니다.
5. OAuth 동의 화면을 생성합니다.

## Conditions (일반 사용자용)
1. 테스트 앱은 사용자 인증 제한(100명)이 있습니다. 서비스 제공자(`devany.kr@gmail.com`)에게 사용하려는 구글 계정(이메일 주소)을 알려줍니다.
    * 사용자에게 `client_secrets.json` 파일을 보내드립니다. `kr.devany.googleapi.sdPlugin\bin` 폴더에 붙여넣으면 됩니다.
2. 계정이 등록되면 서비스를 이용할 수 있습니다.

* 앱이 공식 발행되면 누구나 인증할 수 있습니다.
 
## Working APIs

- ✔️ AdSense Management
- ✔️ Calendar
- ✔️ GMail
- others

## Language
- C#.Net 6

## using NuGet
- Google.Apis.*
- System.*
- Newtonsoft.Json
- [StreamDeck-Tools](https://github.com/BarRaider/streamdeck-tools)
- [DevAny.MAS.Extensions](https://github.com/DevAnyKR/MAS.Libraries)
