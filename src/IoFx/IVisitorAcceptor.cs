﻿namespace IoFx
{
    internal interface IVisitorAcceptor<in T>
    {
        void Accept(T visitor);
    }
}