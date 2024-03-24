# JustWatch Search

This is a project that allows users to search for movies and TV shows on JustWatch.\
It also fetches all available offers for the selected movie or TV show in all countries.

## Live Demo

### [https://nobraincellsleft.github.io/JustWatch-Search/](https://nobraincellsleft.github.io/JustWatch-Search/).


## Docker
[https://ghcr.io/nobraincellsleft/justwatch-search](https://ghcr.io/nobraincellsleft/justwatch-search)
```
docker pull ghcr.io/nobraincellsleft/justwatch-search:latest
```
#VT Api
Tested on windows and has a docker image 
docker image:
```
docker pull luky92/downloadapi
```
you need to setup /vt volume with your vinetrimmer (if you want to setup anther volume set VT_INSTALL_PATH to docker path ypu set your valume to )
in the client app either set DOWNLOAD_API_URL in wwwroot/appsettings.json or set enviroment variable DOWNLOAD_API_URL for docker setup 
config files for the vinetrimmer heave to be in <vt_volume>/vinetrimmer/vinetrimmer.yml and <vt_volume>/vinetrimmer/config/vinetrimmer.yml
cookie dir has to be <vt_volume>/Cookies ( no need to setup the <vt_volume> in the settings for both cases)

NOTE: This program works only with poetry version of vinetrimmer

if your setup is correct you will see a table with configured services in available services tab and a download button in the prices table if you re service is configured in vinetrimmer 

Known issiues:
- no progress or success indicator for downloads 
- Amazon doesnt get detected because of domains missing in shork help text ( can be fixed by editing amazon script and adding missing domains withcommas there) 