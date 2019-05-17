﻿using Autofac;
using MyMovieList.Services;
using MyMovieList.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyMovieList.Configuration
{
    public class IoCConfiguration
    {
        private IContainer container;

        public IoCConfiguration()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ListasViewModel>();
            builder.RegisterType<PeliculasCartelera>();
            builder.RegisterType<PeliculasProximamente>();
            builder.RegisterType<SessionService>();
            this.container = builder.Build();
        }
        public SessionService SessionService
        {
            get { return this.container.Resolve<SessionService>(); }
        }
        public ListasViewModel ListasViewModel
        {
            get
            {
                return this.container.Resolve<ListasViewModel>();
            }
        }

        public PeliculasCartelera PeliculasCartelera
        {
            get
            {
                return this.container.Resolve<PeliculasCartelera>();
            }
        }

        public PeliculasProximamente PeliculasProximamente
        {
            get
            {
                return this.container.Resolve<PeliculasProximamente>();
            }
        }
    }
}
