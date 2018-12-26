wd=$(dirname "$0")

$wd/build.sh win-x64
$wd/build.sh linux-x64
$wd/build.sh linux-arm

DIST_NAME_BASE='Books.NET_'$TRAVIS_TAG
DIST_NAME_WIN=$DIST_NAME_BASE'_win-x64.zip'
DIST_NAME_LINUX=$DIST_NAME_BASE'_linux-x64.tar.gz'
DIST_NAME_ARM=$DIST_NAME_BASE'_linux-arm.tar.gz'

cd $wd/../publish/win-x64
zip -r ../$DIST_NAME_WIN *

cd ../linux-x64
tar -zcvf ../$DIST_NAME_LINUX *

cd ../linux-arm
tar -zcvf ../$DIST_NAME_ARM *

cd ..
ls -lh | grep -v /
