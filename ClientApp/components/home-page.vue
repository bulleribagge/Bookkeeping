<template>
    <div id="app">
        <form>
            <h1>Ladda upp dina filer</h1>
            <div class="dropbox">
                <input type="file" multiple name="files" ref="files" accept="text/plain" v-on:change="handleFilesUpload()" class="input-file">
                <p v-if="!disabled">
                    Dra filerna hit för att ladda upp dem
                </p>
                <p v-if="disabled">
                    Laddar upp filer...
                </p>
            </div>
        </form>
    </div>
</template>
<script>
    import axios from 'axios';

    export default {
        
        data() {
            return {
                files: '',
                disabled: false
            }
        },

        methods: {
            
            submitFiles() {
            
                let formData = new FormData();

                for (var i = 0; i < this.files.length; i++) {
                    let file = this.files[i];
                    formData.append('files' + i, file);
                }
                
                axios.post('/processfiles',
                    formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }
                ).then(function (res) {
                    console.log('SUCCESS!! ', res);
                })
                .catch(function (e) {
                    console.log('FAILURE!! ' + e.message);
                });
            },

            handleFilesUpload() {
                this.disabled = true;
                this.files = this.$refs.files.files;
                this.submitFiles();
            }
        }
    }

</script>

<style>
    .dropbox {
        outline: 2px dashed grey; /* the dash box */
        outline-offset: -10px;
        background: lightcyan;
        color: dimgray;
        padding: 10px 10px;
        min-height: 200px; /* minimum height */
        position: relative;
        cursor: pointer;
    }

    .input-file {
        opacity: 0; /* invisible but it's there! */
        width: 100%;
        height: 200px;
        position: absolute;
        cursor: pointer;
    }

    .dropbox:hover {
        background: lightblue; /* when mouse over to the drop zone, change color */
    }

    .dropbox p {
        font-size: 1.2em;
        text-align: center;
        padding: 50px 0;
    }
</style>
