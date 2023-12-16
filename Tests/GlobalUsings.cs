global using System.Text;
global using System.Linq.Expressions;
global using System.Security.Claims;

global using Xunit;
global using Moq;

global using FluentValidation;
global using FluentValidation.Results;
global using AutoFixture;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;

global using OpenMovies.Controllers;
global using OpenMovies.Repositories;
global using OpenMovies.Services;
global using OpenMovies.Models;
global using OpenMovies.DTOs;
global using OpenMovies.Models.Enums;
global using OpenMovies.Data;
global using OpenMovies.Utils;