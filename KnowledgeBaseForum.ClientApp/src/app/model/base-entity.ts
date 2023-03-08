export class BaseEntity {
    public id!: string;
    public dataCriacao!: Date;
    public dataModificacao?: Date;
    public usuarioCriacao!: string;
    public usuarioModificacao?: string;
}
