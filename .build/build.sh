#!/bin/bash

runtime=$1
skipNpm=$2

if [ -z "$runtime" ]
then
  echo -e "\e[31mError: No runtime provided!\e[0m" 1>&2
  echo "Supported runtimes:" 1>&2
  echo -e "- \e[1;37mwin-x64\e[0m" 1>&2
  echo -e "- \e[1;37mlinux-x64\e[0m" 1>&2
  echo -e "- \e[1;37mlinux-arm\e[0m" 1>&2
  exit -1
fi

if [ -z "$skipNpm" ]
then
  skipNpm=false
fi

wd=$(dirname "$0")

if [ -d "$wd/../publish/$runtime" ]; then
  rm -rf $wd/../publish/$runtime
fi

dotnet publish $wd/../Books.csproj -c Release -r $runtime -o publish/$runtime -p:SkipNpm=$skipNpm --self-contained
code=$?
if [ $code -ne 0 ]
then
  exit $code
fi

if [ -d "$wd/../publish/$runtime/publish" ]; then
  rm -rf $wd/../publish/$runtime/publish
fi
