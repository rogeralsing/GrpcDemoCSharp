FROM microsoft/dotnet

RUN mkdir /usr/app
WORKDIR /usr/app

COPY bin/Release/netcoreapp1.0/publish/ /usr/app
COPY bin/Release/netcoreapp1.0/publish/runtimes/linux/native /usr/app

RUN ln -s /lib/x86_64-linux-gnu/libdl.so.2 /usr/app/libdl.so

EXPOSE 50051

CMD [ "dotnet", "GrpcServer.DotNetCore.dll" ]