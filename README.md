<!--
Based on the README.md template found at https://github.com/othneildrew/Best-README-Template/blob/master/README.md
-->

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![Build Status][build-sheld]][build-url]


<br />

<!-- TABLE OF CONTENTS -->
<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgements">Acknowledgements</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

DiscordWebhookProxy is a webhook endpoint that re-formats the data sent from other services such as GitHub into much better looking and customizable discord webhooks.



**Discord already supports GitHub and Slack style webhooks, why something in between?**

Discord's support for github and slack style webhooks has remained largely unchanged since they were introduced with the platform, leaving meany new features such as GitHub Actions in the dust and silently failing to publish. Additionally, the embeds created by these webhooks often lack important data, or aren't formatted in a way that makes it easy to see important details at a glance. This project aims to fix that.

<!-- GETTING STARTED -->
## Getting Started

Clone and open the project in your favorite IDE/Editor!

### Prerequisites

1. Install Visual Studio Code, Visual Studio 2019, or Rider
1. Install the dotnet SDK
1. Install Docker
1. (Optional) Install docker-compose

### Installation

Command line:

```sh
docker run -d --restart=always -p 80:80 ghcr.io/discord-csharp/discordwebhookproxy:latest
```

Docker Compose:
```yml
version: '3.7'
services:
  repl:
    image: ghcr.io/discord-csharp/discordwebhookproxy:latest
    restart: always
    ports:
      - '80:80'
```

Note: This service isn't configured to handle HTTPS on its own. For HTTPS support, front the container with a reverse proxy such as nginx.

<!-- USAGE EXAMPLES -->
## Usage

```sh
curl -X POST -H 'Content-Type: application/json' -d '{ custom webhook payload }' "http://localhost/webhokproxy/$discordWebhookId/$discordWebhookKey/$platformIdentifier"
```

```pwsh
Invoke-WebRequest -UseBasicParsing -ContentType application/json -Method POST -Body "{ custom webhook payload }" -Uri "http://localhost/webhokproxy/$discordWebhookId/$discordWebhookKey/$platformIdentifier" | Select-Object Content
```


<!-- ROADMAP -->
## Roadmap

See the [open issues](https://github.com/discord-csharp/DiscordWebhookProxy/issues) for a list of proposed features (and known issues).



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE` for more information.



<!-- CONTACT -->
## Contact

Chris Curwick - [@Cisien on Discord](https://discord.gg/csharp)

Project Link: [https://github.com/discord-csharp/DiscordWebhookProxy](https://github.com/discord-csharp/DiscordWebhookProxy)



<!-- ACKNOWLEDGEMENTS -->
## Acknowledgements
* [Remora.Discord](https://github.com/Nihlus/Remora.Discord)
* [dotnet](https://github.com/dotnet)
* [.NET Foundation](https://dotnetfoundation.org)
* [Img Shields](https://shields.io)



<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/discord-csharp/DiscordWebhookProxy.svg?style=for-the-badge
[contributors-url]: https://github.com/discord-csharp/DiscordWebhookProxy/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/discord-csharp/DiscordWebhookProxy.svg?style=for-the-badge
[forks-url]: https://github.com/discord-csharp/DiscordWebhookProxy/network/members
[stars-shield]: https://img.shields.io/github/stars/discord-csharp/DiscordWebhookProxy.svg?style=for-the-badge
[stars-url]: https://github.com/discord-csharp/DiscordWebhookProxy/stargazers
[issues-shield]: https://img.shields.io/github/issues/discord-csharp/DiscordWebhookProxy.svg?style=for-the-badge
[issues-url]: https://github.com/discord-csharp/DiscordWebhookProxy/issues
[license-shield]: https://img.shields.io/github/license/discord-csharp/DiscordWebhookProxy.svg?style=for-the-badge
[license-url]: https://github.com/discord-csharp/DiscordWebhookProxy/blob/master/LICENSE.txt
[build-sheld]: https://img.shields.io/github/workflow/status/discord-csharp/DiscordWebhookProxy/build-csharprepl?style=for-the-badge
[build-url]: https://github.com/discord-csharp/DiscordWebhookProxy/actions/workflows/build-container.yml
