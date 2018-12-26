wd=$(dirname "$0")

$wd/build.sh win-x64
$wd/build.sh linux-x64
$wd/build.sh linux-arm

cd $wd/../publish/win-x64
zip -r ../Books.NET_win-x64.zip *

cd ../linux-x64
tar -zcvf ../Books.NET_linux-x64.tar.gz *

cd ../linux-arm
tar -zcvf ../Books.NET_linux-arm.tar.gz *
