<template>
    <div id="app">
        <form id="uploadForm" v-if="!doneUploading">
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
        <div v-if="doneUploading">
            <h1>
                Transaktioner
            </h1>
            <table>
                <thead>
                    <tr>
                        <th>
                            Datum
                        </th>
                        <th>
                            Typ
                        </th>
                        <th>
                            Namn
                        </th>
                        <th>
                            Belopp inkl moms
                        </th>
                        <th>
                            Belopp exl moms
                        </th>
                        <th>
                            Fakturanummer
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(transaction, index) in transactions">
                        <td>
                            {{transaction.registerDate | shortDate}}
                        </td>
                        <td>
                            {{transaction.transactionType}}
                        </td>
                        <td>
                            {{transaction.reference}}
                        </td>
                        <td>
                            {{transaction.amount}}
                        </td>
                        <td>
                            <span v-if="transaction.transactionType === 'SWISH'">
                                {{transaction.amountExludingVAT}}
                            </span>
                        </td>
                        <td>
                            {{year + (parseInt(firstInvoiceNumber) + index)}}
                        </td>
                    </tr>
                </tbody>
            </table>
            <hr />
            <div id="inputDiv">
                <div>
                    <label for="year">Ange år: </label><input type="text" name="year" id="year" v-model="year" class="smallInput" />
                </div>
                <div>
                    <label for="firstInvoiceNumber">Ange första fakturanummer: </label> <input type="text" name="firstInvoiceNumber" id="firstInvoiceNumber" v-model="firstInvoiceNumber" class="smallInput" />
                </div>
                    <div class="clearFix"></div>
                </div>
        </div>
    </div>
</template>
<script>
    import axios from 'axios';

    export default {
        
        data() {
            return {
                files: '',
                disabled: false,
                doneUploading: false,
                transactions: [],
                year: 0,
                firstInvoiceNumber: 0
            }
        },

        methods: {
            
            submitFiles() {
            
                let formData = new FormData();

                for (var i = 0; i < this.files.length; i++) {
                    let file = this.files[i];
                    formData.append('files' + i, file);
                }
                let self = this;
                axios.post('/processfiles',
                    formData,
                    {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }
                ).then(function (res) {
                    console.table(res.data);
                    self.doneUploading = true;
                    self.transactions = res.data;
                })
                .catch(function (e) {
                    console.log('Error ' + e.message);
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

    table td, th {
        padding: 5px;
    }

    table{
        margin-bottom: 20px;
    }

    .smallInput{
        width: 50px;
        float: right;
    }

    #inputDiv{
        width: 300px;
    }

    #inputDiv div{
        margin: 1px;
    }

    .clearFix{
        clear:both;
    }

</style>
