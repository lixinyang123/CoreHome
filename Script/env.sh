a=`uname  -a`

b="Darwin"
c="Centos"
d="Ubuntu"

if [[ $a =~ $b ]];then
    echo "mac"
elif [[ $a =~ $c ]];then
    echo "centos"
elif [[ $a =~ $d ]];then
    echo "ubuntu"
else
    echo $a
fi
