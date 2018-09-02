FROM microsoft/aspnetcore-build

WORKDIR /home/app

COPY ./Bookkeeping.csproj .

RUN dotnet restore

COPY . .

RUN dotnet publish ./Bookkeeping.csproj -o /publish/

WORKDIR /publish

ENTRYPOINT ["dotnet", "Bookkeeping.dll"]
