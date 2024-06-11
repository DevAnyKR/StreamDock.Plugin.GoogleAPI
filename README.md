# GoogleAPI Plugin for Stream Dock

스트림독에서 구글 API 기능을 사용할 수 있습니다. 보안이 우수한 OAuth 2.0으로 구글 계정을 인증합니다.

## Sample

![image](https://github.com/DevAnyKR/StreamDock.Plugin.GoogleAPI/assets/110871727/e4952131-0713-429c-8385-70e0d2d2fac4)

![image](https://github.com/DevAnyKR/StreamDock.Plugin.GoogleAPI/assets/110871727/012867af-0371-4beb-9158-0c12673701d9)


## How to Install...

0. 스트림독을 종료합니다.
1. VS에서 컴파일하거나 릴리스 파일을 내려받아 압축을 풉니다.
2. kr.devany.googleapi.adsense.sdPlugin 폴더를 %AppData%\HotSpot\StreamDock\plugins\ 위치에 복사합니다.
3. 스트림독을 실행합니다.

## Conditions

- 사용자는 필요한 기능의 [Google API](https://console.cloud.google.com/) 키를 발급받아야 합니다. Google Cloud에서 만들 수 있습니다.
- 원본과 다른 이름으로 프로젝트 서비스를 하려면 사용자 인증 정보(OAuth 2.0 클라이언트 ID)를 만들어야 합니다.

 ### Google API

1. 구글 콘솔에 접속합니다. [Google Console](https://console.cloud.google.com/) 
2. 새 프로젝트를 생성합니다.
3. 사용하려는 API 키를 발급합니다.
    * AdSense Management
4. 테스트 앱이므로 사용자 인증 제한이 있습니다. 개발자에게 사용하려는 구글 계정(이메일 주소)을 알려줍니다.
5. 계정이 등록되면 서비스를 이용할 수 있습니다.

* 앱이 공식 발행되면 누구나 인증할 수 있습니다.
 
## Working APIs

- AdSense Management (2024-06-07~)

## Todo APIs

- GMail

## using NuGet

- Google.Apis
- Google.Apis.Adsense.v2
- Google.Apis.Auth
- Newtonsoft.Json
- StreamDeck-Tools
