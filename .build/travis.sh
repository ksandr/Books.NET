wd=$(dirname "$0")

cd $wd/../Books/ClientApp
npm install
npm run build
code=$?
  if [ $code -ne 0 ]
  then
    exit $code
  fi

cd ../../.build
runtimes=(win-x64 linux-x64 linux-arm)
for runtime in ${runtimes[*]}
do
  ./build.sh $runtime true
  code=$?
  if [ $code -ne 0 ]
  then
    exit $code
  fi
done

DIST_NAME_BASE='Books.NET_'$TRAVIS_TAG
DIST_NAME_WIN=$DIST_NAME_BASE'_win-x64.zip'
DIST_NAME_LINUX=$DIST_NAME_BASE'_linux-x64.tar.gz'
DIST_NAME_ARM=$DIST_NAME_BASE'_linux-arm.tar.gz'

cd ../publish/win-x64
zip -r ../$DIST_NAME_WIN *

cd ../linux-x64
tar -zcvf ../$DIST_NAME_LINUX *

cd ../linux-arm
tar -zcvf ../$DIST_NAME_ARM *

cd ..
ls -lh | grep -v /
