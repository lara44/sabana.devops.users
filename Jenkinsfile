pipeline {
    /**
     * AGENTE DE EJECUCIÓN
     * Equivalente a "runs-on: ubuntu-latest" en GitHub Actions.
     * En lugar de instalar .NET con actions/setup-dotnet, usamos directamente
     * la imagen oficial del SDK de .NET 10 como contenedor de ejecución.
     *
     * El flag -v monta el socket de Docker del host dentro del contenedor,
     * lo que permite ejecutar comandos "docker build/push" desde adentro
     * (técnica conocida como Docker-in-Docker o DinD por socket).
     */
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:10.0'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }

    /**
     * TRIGGERS AUTOMÁTICOS
     * Equivalente al bloque "on: push / pull_request" de GitHub Actions.
     * githubPush() escucha eventos de push vía webhook configurado en el repo.
     * Requiere el plugin "GitHub" instalado en Jenkins.
     */
    triggers {
        githubPush()
    }

    /**
     * VARIABLES DE ENTORNO Y CREDENCIALES
     * Equivalente a los "secrets" de GitHub Actions (${{ secrets.X }}).
     * credentials() recupera valores almacenados de forma segura en
     * Manage Jenkins → Credentials. Nunca se exponen en texto plano en los logs.
     *
     * SOLUTION e IMAGE_NAME son constantes reutilizables a lo largo del pipeline
     * para evitar hardcodear strings repetidos en cada stage.
     */
    environment {
        DOCKERHUB_USERNAME = credentials('DOCKERHUB_USERNAME')
        DOCKERHUB_TOKEN    = credentials('DOCKERHUB_TOKEN')
        SOLUTION           = 'sabana.devops.users.sln'
        IMAGE_NAME         = "${DOCKERHUB_USERNAME}/sabana-devops-users:latest"
    }

    stages {
        /**
         * CHECKOUT
         * Equivalente a "uses: actions/checkout@v4".
         * "checkout scm" clona automáticamente el repositorio y la rama
         * que disparó el pipeline, usando la configuración del job en Jenkins.
         */
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        /**
         * RESTAURAR DEPENDENCIAS
         * Equivalente al step "Restore dependencies" de GitHub Actions.
         * Descarga los paquetes NuGet definidos en el .sln antes de compilar,
         * separado del build para detectar errores de dependencias de forma aislada.
         */
        stage('Restore dependencies') {
            steps {
                sh 'dotnet restore ${SOLUTION}'
            }
        }

        /**
         * COMPILACIÓN
         * Equivalente al step "Build".
         * --no-restore evita una restauración redundante (ya se hizo en el stage anterior).
         * --configuration Release genera artefactos optimizados para producción.
         */
        stage('Build') {
            steps {
                sh 'dotnet build ${SOLUTION} --no-restore --configuration Release'
            }
        }

        /**
         * PRUEBAS
         * Equivalente al step "Test".
         * --no-build evita recompilar (reutiliza los artefactos del stage anterior).
         * --verbosity normal muestra resultados detallados de cada test en los logs.
         * El pipeline se detiene automáticamente aquí si algún test falla.
         */
        stage('Test') {
            steps {
                sh 'dotnet test ${SOLUTION} --no-build --verbosity normal --configuration Release'
            }
        }

        /**
         * DOCKER LOGIN & PUSH
         * Equivalente a los steps "Login to Docker Hub" y "Build and push Docker image",
         * ambos con el condicional "if: github.ref == 'refs/heads/main'".
         *
         * CONDICIÓN when { branch 'main' }:
         * Este stage SOLO se ejecuta cuando el pipeline corre sobre la rama main,
         * ignorándose en Pull Requests u otras ramas. Es el equivalente directo
         * del condicional "if" de GitHub Actions.
         *
         * LOGIN SEGURO:
         * Se usa --password-stdin en lugar de pasar el token directamente en el comando,
         * evitando que la contraseña quede expuesta en los logs del proceso.
         *
         * BUILD & PUSH:
         * Construye la imagen usando el Dockerfile en la raíz del repo y la publica
         * en Docker Hub bajo el tag "latest".
         */
        stage('Docker Login & Push') {
            when {
                branch 'main'
            }
            steps {
                sh '''
                    echo "$DOCKERHUB_TOKEN" | docker login -u "$DOCKERHUB_USERNAME" --password-stdin
                    docker build -t ${IMAGE_NAME} .
                    docker push ${IMAGE_NAME}
                '''
            }
        }
    }

    /**
     * ACCIONES POST-EJECUCIÓN
     * Equivalente a los "post" steps o actions de limpieza en GitHub Actions.
     *
     * always:  Se ejecuta sin importar si el pipeline tuvo éxito o falló.
     *          cleanWs() elimina los archivos del workspace para liberar espacio en disco.
     * success: Solo se ejecuta si todos los stages completaron sin errores.
     * failure: Solo se ejecuta si algún stage falló; ideal para enviar notificaciones
     *          (Slack, email, etc.) en caso de rotura del pipeline.
     */
    post {
        always {
            cleanWs()
        }
        success {
            echo 'Pipeline completado exitosamente.'
        }
        failure {
            echo 'El pipeline falló.'
        }
    }
}