pipeline {
    agent any

    tools {
        // .NET SDK version installed on Jenkins agent
        dotnet 'dotnet-8.0'
    }

    

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url: 'https://github.com/your-repo.git'
            }
        }

        stage('Restore') {
            steps {
                sh 'dotnet restore'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet build --configuration $BUILD_CONFIGURATION'
            }
        }

        // stage('Test') {
        //     steps {
        //         sh 'dotnet test'
        //     }
        // }

        // stage('Publish') {
        //     steps {
        //         sh 'dotnet publish --configuration $BUILD_CONFIGURATION -o ./publish'
        //     }
        // }

        // stage('Deploy') {
        //     steps {
        //         // Example for copying files to a server
        //         sh 'cp -r ./publish/* /var/www/aspnetapp/'
        //     }
        }
    }

    post {
        success {
            echo 'Build and deployment successful!'
        }
        failure {
            echo 'Build failed!'
        }
    }
}
